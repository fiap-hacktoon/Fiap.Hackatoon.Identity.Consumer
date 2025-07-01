using FIAP.TechChallenge.UserHub.Domain.Entities;

namespace FIAP.TechChallenge.UserHub.UnitTests
{
    public static class CommonTestData
    {
        public static Client GetClientObject()
            => new()
            {
                Id = 1,
                TypeRole = 0,
                Name = "Marcelo Cedro",
                Document = "982878151",
                Email = "wagnergarniz@gmail.com",
                Birth = DateOnly.MaxValue,
                Creation = DateTime.Now,
                Password = "password"
            };

        public static List<Client> GetClientListObject()
            =>
            [
                new ()
                {
                    Id = 1,
                    TypeRole = 1,
                    Name = "Carter Grayson",
                    Document = "982878151",
                    Email = "carter@gmail.com",
                    Birth = DateOnly.MaxValue,
                    Creation = DateTime.Now,
                    Password = "password"
                },
                new ()
                {
                    Id = 1,
                    TypeRole = 2,
                    Name = "Marcelo Cedro",
                    Document = "982840611",
                    Email = "marceloced@gmail.com",
                    Birth = DateOnly.MaxValue,
                    Creation = DateTime.Now,
                    Password = "password"
                }
            ];

        public static Employee GetEmployeeObject()
            => new()
            {
                Id = 1,
                Name = "Marcelo Cedro",
                TypeRole = (int)Domain.Enumerators.TypeRole.Manager,
                Email = "wagnergarniz@gmail.com",
                Creation = DateTime.Now,
                Password= "password"
            };

        public static List<Employee> GetEmployeeListObject()
            =>
            [
                new ()
                {
                    Id = 1,
                    Name = "Carter Grayson",
                    TypeRole = (int)Domain.Enumerators.TypeRole.Kitchen,
                    Email = "carter@gmail.com",
                    Creation = DateTime.Now,
                    Password= "password"
                },
                new ()
                {
                    Id = 1,
                    Name = "Marcelo Cedro",
                    TypeRole = (int)Domain.Enumerators.TypeRole.Manager,
                    Email = "marceloced@gmail.com",
                    Creation = DateTime.Now,
                    Password= "password"
                }
            ];
    }
}
