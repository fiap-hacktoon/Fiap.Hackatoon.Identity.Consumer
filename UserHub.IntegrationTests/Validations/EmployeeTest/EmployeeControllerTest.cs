using FIAP.TechChallenge.UserHub.Api.Controllers;
using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using FIAP.TechChallenge.UserHub.Domain.Services;
using FIAP.TechChallenge.UserHub.Infrastructure.Repositories;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.EmployeeTest
{
    public class EmployeeControllerTest : BaseServiceTests
    {
        private readonly EmployeeController _contactsController;
        private readonly IEmployeeApplication _contactApplication;
        private readonly IEmployeeService _contactService;
        private Mock<ILogger<EmployeeService>> _loggerServiceMock;
        private Mock<ILogger<EmployeeApplication>> _loggerApplicationMock;
        private Mock<ILogger<EmployeeController>> _loggerControllerMock;
        private readonly IEmployeeRepository _contactRepository;
        public readonly Random RandomId;

        public EmployeeControllerTest()
        {            
            _contactRepository = new EmployeeRepository(_context);
            _loggerServiceMock = new Mock<ILogger<EmployeeService>>();
            _loggerApplicationMock = new Mock<ILogger<EmployeeApplication>>();
            _loggerControllerMock = new Mock<ILogger<EmployeeController>>();
            _contactService = new EmployeeService(_contactRepository, _loggerServiceMock.Object);
            _contactApplication = new EmployeeApplication(_contactService, _loggerApplicationMock.Object);
            _contactsController = new EmployeeController(_contactApplication, _loggerControllerMock.Object);
            RandomId = new Random();
        }

        [Fact]
        public async Task GetAllContactSuccessAsync()
        {
            await ConfigureInMemoryDatabase();

            var resultValue = await _contactsController.GetAllContacts();

            Assert.NotNull(resultValue);
            Assert.Equal(3, resultValue.Count());
            Assert.NotEmpty(resultValue);
        }

        [Fact]
        public async Task GetContactByIdSuccessAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var contact = inMemoryList.FirstOrDefault();
            var resultValue = await _contactsController.GetContactById(contact.Id);

            Assert.NotNull(resultValue.Value);
            Assert.Equal(contact.Id, resultValue.Value.Id);
        }

        [Fact]
        public async Task GetContactByIdNotFoundAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var resultValue = await _contactsController.GetContactById(250493);

            Assert.Null(resultValue.Value);
        }

        //[Fact]
        //public async Task GetContactByAreaCodeSuccessAsync()
        //{
        //    var inMemoryList = await ConfigureInMemoryDatabase();
        //    var contact = inMemoryList.FirstOrDefault();
        //    var resultValue = await _contactsController.GetContactsByDDD(contact.AreaCode);

        //    Assert.NotEmpty(resultValue);
        //    var listIds = inMemoryList.Select(x => x.Id);

        //    Assert.Contains(contact.Id, listIds);
        //}

        //[Fact]
        //public async Task GetContactByAreaCodeNotFoundAsync()
        //{
        //    var inMemoryList = await ConfigureInMemoryDatabase();
        //    var resultValue = await _contactsController.GetContactsByDDD("501");

        //    Assert.Empty(resultValue);
        //}

        private async Task<List<Employee>> ConfigureInMemoryDatabase()
        {
            var returnContactList = new List<Employee>();
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

            returnContactList.Add(contact1);
            returnContactList.Add(contact2);
            returnContactList.Add(contact3);

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            return returnContactList;
        }

        [Fact]
        public async Task GetContactByEmailSuccessAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var contact = inMemoryList.FirstOrDefault();
            var resultValue = await _contactsController.GetContactByEmailAsync(contact.Email);

            Assert.NotNull(resultValue);
            Assert.Equal(contact.Email, resultValue.Value.Email);
        }

        [Fact]
        public async Task GetContactByEmailNotFoundAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var resultValue = await _contactsController.GetContactByEmailAsync("testejamaismapeado@hotmail.com");

            Assert.Null(resultValue.Value);
        }
    }
}
