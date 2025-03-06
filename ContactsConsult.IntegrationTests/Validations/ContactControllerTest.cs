using FIAP.TechChallenge.ContactsConsult.Api.Controllers;
using FIAP.TechChallenge.ContactsConsult.Application.Applications;
using FIAP.TechChallenge.ContactsConsult.Domain.Entities;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Applications;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Services;
using FIAP.TechChallenge.ContactsConsult.Domain.Services;
using FIAP.TechChallenge.ContactsConsult.Infrastructure.Repositories;
using FIAP.TechChallenge.ContactsConsult.IntegrationTest.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ContactsConsult.IntegrationTest.Validations
{
    public class ContactControllerTest : BaseServiceTests
    {
        private readonly ContactsController _contactsController;
        private readonly IContactApplication _contactApplication;
        private readonly IContactService _contactService;
        private Mock<ILogger<ContactService>> _loggerServiceMock;
        private Mock<ILogger<ContactApplication>> _loggerApplicationMock;
        private Mock<ILogger<ContactsController>> _loggerControllerMock;
        private readonly IContactRepository _contactRepository;
        public readonly Random RandomId;

        public ContactControllerTest()
        {            
            _contactRepository = new ContactRepository(_context);
            _loggerServiceMock = new Mock<ILogger<ContactService>>();
            _loggerApplicationMock = new Mock<ILogger<ContactApplication>>();
            _loggerControllerMock = new Mock<ILogger<ContactsController>>();
            _contactService = new ContactService(_contactRepository, _loggerServiceMock.Object);
            _contactApplication = new ContactApplication(_contactService, _loggerApplicationMock.Object);
            _contactsController = new ContactsController(_contactApplication, _loggerControllerMock.Object);
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

        [Fact]
        public async Task GetContactByAreaCodeSuccessAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var contact = inMemoryList.FirstOrDefault();
            var resultValue = await _contactsController.GetContactsByDDD(contact.AreaCode);

            Assert.NotEmpty(resultValue);
            var listIds = inMemoryList.Select(x => x.Id);

            Assert.Contains(contact.Id, listIds);
        }

        [Fact]
        public async Task GetContactByAreaCodeNotFoundAsync()
        {
            var inMemoryList = await ConfigureInMemoryDatabase();
            var resultValue = await _contactsController.GetContactsByDDD("501");

            Assert.Empty(resultValue);
        }

        private async Task<List<Contact>> ConfigureInMemoryDatabase()
        {
            var returnContactList = new List<Contact>();
            var contact1 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));

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
