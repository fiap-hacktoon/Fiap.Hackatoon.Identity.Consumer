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

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.ClientTest
{
    public class ClientControllerTest : BaseServiceTests
    {
        private readonly ClientController _contactsController;
        private readonly IClientApplication _contactApplication;
        private readonly IClientService _contactService;
        private Mock<ILogger<ClientService>> _loggerServiceMock;
        private Mock<ILogger<ClientApplication>> _loggerApplicationMock;
        private Mock<ILogger<ClientController>> _loggerControllerMock;
        private readonly IClientRepository _contactRepository;
        public readonly Random RandomId;

        public ClientControllerTest()
        {            
            _contactRepository = new ClientRepository(_context);
            _loggerServiceMock = new Mock<ILogger<ClientService>>();
            _loggerApplicationMock = new Mock<ILogger<ClientApplication>>();
            _loggerControllerMock = new Mock<ILogger<ClientController>>();
            _contactService = new ClientService(_contactRepository, _loggerServiceMock.Object);
            _contactApplication = new ClientApplication(_contactService, _loggerApplicationMock.Object);
            _contactsController = new ClientController(_contactApplication, _loggerControllerMock.Object);
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

        private async Task<List<Client>> ConfigureInMemoryDatabase()
        {
            var returnContactList = new List<Client>();
            var contact1 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));

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
