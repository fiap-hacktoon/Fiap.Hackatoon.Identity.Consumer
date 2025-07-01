using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FIAP.TechChallenge.UserHub.Application
{
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplicationDependency(this IServiceCollection service)
        {
            service.AddScoped<IEmployeeApplication, EmployeeApplication>();
            service.AddScoped<IClientApplication, ClientApplication>();
            service.AddScoped<ITokenApplication, TokenApplication>();

            return service;
        }

        public static IServiceCollection AddAuthenticationDependency(this IServiceCollection service)
        {
            var _configuration = new ConfigurationBuilder()
                                 .AddJsonFile("appsettings.json")
                                 .Build();

            var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT") ?? string.Empty);

            service.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(chaveCriptografia),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            return service;
        }
    }
}
