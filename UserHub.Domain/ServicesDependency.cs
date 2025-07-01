using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using FIAP.TechChallenge.UserHub.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.UserHub.Domain
{
    public static class ServicesDependency
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection service)
        {
            service.AddScoped<IClientService, ClientService>();
            service.AddScoped<IEmployeeService, EmployeeService>();

            return service;
        }
    }
}
