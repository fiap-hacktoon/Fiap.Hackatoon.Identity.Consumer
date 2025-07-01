using FIAP.TechChallenge.UserHub.Application;
using FIAP.TechChallenge.UserHub.Domain;
using FIAP.TechChallenge.UserHub.Infrastructure;

namespace FIAP.TechChallenge.UserHub.Api.IoC
{
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services, string connectionString)
        {
            services.AddRepositoriesDependency();
            services.AddDbContextDependency(connectionString);
            services.AddServicesDependency();
            services.AddApplicationDependency();
            services.AddAuthenticationDependency();
        }
    }
}
