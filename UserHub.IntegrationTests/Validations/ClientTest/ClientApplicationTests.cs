using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using FIAP.TechChallenge.UserHub.Domain.Services;
using FIAP.TechChallenge.UserHub.Infrastructure.Repositories;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.ClientTest
{
    public class ClientApplicationTests : BaseServiceTests
    {
        private readonly IClientService _contactService;
        private readonly IClientApplication _contactApplicationException;
        private readonly IClientApplication _contactApplication;
        private readonly IClientRepository _contactRepository;
        private Mock<ILogger<ClientService>> _loggerServiceMock;
        private Mock<ILogger<ClientApplication>> _loggerApplicationMock;
        public readonly Random RandomId;
        private Mock<IElasticClient<Client>> _elasticClientMock;

        public ClientApplicationTests()
        {
            _contactRepository = new ClientRepository(_context);
            _loggerServiceMock = new Mock<ILogger<ClientService>>();
            _loggerApplicationMock = new Mock<ILogger<ClientApplication>>();
            _contactService = new ClientService(_contactRepository, _loggerServiceMock.Object);
            _elasticClientMock = new Mock<IElasticClient<Client>>();
            _contactApplication = new ClientApplication(_contactService, _loggerApplicationMock.Object, _elasticClientMock.Object);
            _contactApplicationException = new ClientApplication(null, _loggerApplicationMock.Object, _elasticClientMock.Object);
            RandomId = new Random();
        }

        [Fact]
        public async Task GetAllContactSuccessAsync()
        {
            var contact1 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplication.GetAllClientsAsync();
            Assert.NotNull(contactList);
            Assert.NotEmpty(contactList);
        }

        [Fact]
        public async Task GetAllContactExceptionAsync()
        {
            var contact1 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplicationException.GetAllClientsAsync();
            Assert.Null(contactList);
        }

        [Fact]
        public async Task GetContactByIdExceptionAsync()
        {
            var contact1 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplicationException.GetClientByIdAsync(contact2.Id);
            Assert.Null(contactList);
        }

        [Fact]
        public async Task GetContactByIdSuccessAsync()
        {
            var contact1 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ClientFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplication.GetClientByIdAsync(contact2.Id);
            Assert.NotNull(contactList);
            Assert.Equal(contactList.Id, contact2.Id);
        }
    }
}
