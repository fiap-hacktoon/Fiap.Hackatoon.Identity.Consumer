using FIAP.TechChallenge.ContactsConsult.Domain.Entities;

namespace FIAP.TechChallenge.ContactsConsult.UnitTests
{
    public static class CommonTestData
    {
        public static Contact GetContactObject()
            => new()
            {
                Id = 1,
                Name = "Marcelo Cedro",
                AreaCode = "11",
                Phone = "982878151",
                Email = "wagnergarniz@gmail.com"
            };

        public static List<Contact> GetContacListObject()
            =>
            [
                new ()
                {
                    Id = 1,
                    Name = "Carter Grayson",
                    AreaCode = "11",
                    Phone = "982878151",
                    Email = "carter@gmail.com"
                },
                new ()
                {
                    Id = 1,
                    Name = "Marcelo Cedro",
                    AreaCode = "11",
                    Phone = "982840611",
                    Email = "marceloced@gmail.com"
                }
            ];

    }
}
