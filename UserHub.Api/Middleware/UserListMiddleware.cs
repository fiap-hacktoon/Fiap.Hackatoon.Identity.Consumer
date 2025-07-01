using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace FIAP.TechChallenge.UserHub.Api.Middleware
{
    public class UserListMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public Task Invoke(HttpContext httpContext)
        {
            EmployeeList.Users = [
                                 new Employee { Id = 1, Name = "admin", Password = "admin", TypeRole = (int)TypeRole.Manager, Email = "email@email.com.br", Creation = DateTime.Now },
                                 new Employee { Id = 2, Name = "user", Password = "user",  TypeRole = (int)TypeRole.Attendant, Email = "email@email.com.br", Creation = DateTime.Now },
                                 new Employee { Id = 3, Name = "guest", Password = "guest", TypeRole = (int)TypeRole.Kitchen, Email = "email@email.com.br", Creation = DateTime.Now }
                             ];

            return _next(httpContext);
        }
    }

    public static class UserListMiddlewareExtensions
    {
        public static IApplicationBuilder UseListaUserMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<UserListMiddleware>();
    }
}
