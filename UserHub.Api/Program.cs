using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Api.IoC;
using FIAP.TechChallenge.UserHub.Api.Logging;
using FIAP.TechChallenge.UserHub.Api.Middleware;
using MassTransit;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyResolver(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().ForwardToPrometheus();

builder.Logging.ClearProviders();
builder.Logging.AddProvider(
    new CustomLoggerProvider(
        new CustomLoggerProviderConfiguration
        {
            LogLevel = LogLevel.Information,
        }));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechChallenge Terceiro FIAP 2025", Version = "v1" });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var teste = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";

    var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Cabeçalho de autorização JWT usando o esquema Bearer. 
                        Insira 'Bearer' [espaço] e, em seguida, seu token na entrada de texto abaixo.
                        Exemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            Array.Empty<string>()
        }
    });
});

IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                         .AddJsonFile("appsettings.json", false, true)
                                                         .AddJsonFile($"appsettings.Dev.json", true, true)
                                                         .AddEnvironmentVariables()
                                                         .Build();
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddHealthChecks().ForwardToPrometheus();

var queueClientCreate = configuration.GetSection("MassTransit:QueueNameClienteCreate").Value ?? string.Empty;
var queueClientUpdate = configuration.GetSection("MassTransit:QueueNameClienteUpdate").Value ?? string.Empty;
var queueEmployeeCreate = configuration.GetSection("MassTransit:QueueNameEmployeeCreate").Value ?? string.Empty;
var queueEmployeeUpdate = configuration.GetSection("MassTransit:QueueNameEmployeeUpdate").Value ?? string.Empty;
var server = configuration.GetSection("MassTransit:Server").Value ?? string.Empty;
var user = configuration.GetSection("MassTransit:User").Value ?? string.Empty;
var password = configuration.GetSection("MassTransit:Password").Value ?? string.Empty;

builder.Services.AddMassTransit(x =>
{
    // Registra todos os consumers
    x.AddConsumer<ClientAddConsumer>();
    x.AddConsumer<ClientUpdateConsumer>();
    x.AddConsumer<EmployeeAddConsumer>();
    x.AddConsumer<EmployeeUpdateConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(server, "/", h =>
        {
            h.Username(user);
            h.Password(password);
        });

        // Configura fila e consumer do CLIENTE - CREATE
        cfg.ReceiveEndpoint(queueClientCreate, e =>
        {
            e.ConfigureConsumer<ClientAddConsumer>(context);
        });

        // Configura fila e consumer do CLIENTE - UPDATE
        cfg.ReceiveEndpoint(queueClientUpdate, e =>
        {
            e.ConfigureConsumer<ClientUpdateConsumer>(context);
        });

        // Configura fila e consumer do EMPLOYEE - CREATE
        cfg.ReceiveEndpoint(queueEmployeeCreate, e =>
        {
            e.ConfigureConsumer<EmployeeAddConsumer>(context);
        });

        // Configura fila e consumer do EMPLOYEE - UPDATE
        cfg.ReceiveEndpoint(queueEmployeeUpdate, e =>
        {
            e.ConfigureConsumer<EmployeeUpdateConsumer>(context);
        });
    });
});


//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<ClientAddConsumer>();
//    x.AddConsumer<ClientUpdateConsumer>();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host(server, "/", h =>
//        {
//            h.Username(user);
//            h.Password(password);
//        });

//        cfg.ReceiveEndpoint(queueClientCreate, e =>
//        {
//            e.ConfigureConsumer<ClientAddConsumer>(context);
//        });
//        cfg.ReceiveEndpoint(queueClientUpdate, e =>
//        {
//            e.ConfigureConsumer<ClientUpdateConsumer>(context);
//        });
//    });
//});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseListaUserMiddleware();

app.UseHealthChecks("/health");
app.UseHttpMetrics();
app.MapMetrics();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
