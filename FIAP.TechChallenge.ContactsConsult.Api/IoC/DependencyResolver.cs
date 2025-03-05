using FIAP.TechChallenge.ContactsConsult.Application;
using FIAP.TechChallenge.ContactsConsult.Domain;
using FIAP.TechChallenge.ContactsConsult.Infrastructure;

namespace FIAP.TechChallenge.ContactsConsult.Api.IoC
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
