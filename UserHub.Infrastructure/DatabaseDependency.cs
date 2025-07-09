using FFIAP.TechChallenge.UserHub.Infrastructure.ElasticSearch;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Infrastructure.Data;
using FIAP.TechChallenge.UserHub.Infrastructure.ElasticSearch;
using FIAP.TechChallenge.UserHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.UserHub.Infrastructure
{
    public static class DatabaseDependency
    {
        public static IServiceCollection AddRepositoriesDependency(this IServiceCollection service)
        {
            service.AddScoped<IClientRepository, ClientRepository>();
            service.AddScoped<IEmployeeRepository, EmployeeRepository>();

      
            service.AddSingleton<IElasticSettings>(sp => sp.GetRequiredService<IOptions<ElasticSettings>>().Value);
            service.AddSingleton(typeof(IElasticClient<>), typeof(ElasticClient<>));

            return service;
        }

        public static IServiceCollection AddDbContextDependency(this IServiceCollection service, string connectionString)
        {
            service.AddDbContext<ContactsDbContext>(options => options.UseSqlServer(connectionString));

            return service;
        }
    }
}
