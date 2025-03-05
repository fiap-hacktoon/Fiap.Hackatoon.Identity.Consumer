using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Services;
using FIAP.TechChallenge.ContactsConsult.Domain.Services;
using FIAP.TechChallenge.ContactsConsult.Infrastructure.Repositories;
using FIAP.TechChallenge.ContactsConsult.IntegrationTest.Config;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ContactsConsult.IntegrationTest.Validations
{
    public class ContactServiceTests : BaseServiceTests
    {
        private readonly IContactService _contactService;
        private readonly IContactService _contactServiceException;
        private readonly IContactRepository _contactRepository;
        private Mock<ILogger<ContactService>> _loggerMock;
        public readonly Random RandomId;

        public ContactServiceTests()
        {
            _contactRepository = new ContactRepository(_context);
            _loggerMock = new Mock<ILogger<ContactService>>();
            _contactService = new ContactService(_contactRepository, _loggerMock.Object);
            _contactServiceException = new ContactService(null, _loggerMock.Object);
            RandomId = new Random();
        }

        [Fact]
        public async Task GetByIdContactSuccessAsync()
        {
            var contact1 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2);

            await SaveChanges();

            var foundRecord = await _contactService.GetByIdAsync(contact2.Id);

            Assert.NotNull(foundRecord);
            Assert.Equal(foundRecord.Id, contact2.Id);
        }

        [Fact]
        public async Task GetByIdContactNotFoundAsync()
        {
            var foundRecord = await _contactService.GetByIdAsync(RandomId.Next(999999999));
            Assert.Null(foundRecord);
        }

        [Fact]
        public async Task GetByIdContactExceptionAsync()
        {
            var contact = ContactFixtures.CreateFakeContact(0);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetByIdAsync(contact.Id));
            Assert.Equal($"Some error occour when trying to get a contact with Id: {contact.Id} Contact.", exception.Message);
        }

        [Fact]
        public async Task GetAllContactSuccessAsync()
        {
            var contact1 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var foundRecords = await _contactService.GetAllAsync();

            Assert.NotNull(foundRecords);
            Assert.NotEmpty(foundRecords);
        }

        [Fact]
        public async Task GetAllContactNotFoundAsync()
        {
            var foundRecords = await _contactService.GetAllAsync();
            Assert.Empty(foundRecords);
        }

        [Fact]
        public async Task GetAllContactExceptionAsync()
        {
            var contact = ContactFixtures.CreateFakeContact(0);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetAllAsync());
            Assert.Equal($"Some error occour when trying to get all contacts in database.", exception.Message);
        }

        [Fact]
        public async Task GetByAreaCodeSuccessAsync()
        {
            var contact1 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = ContactFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var foundRecords = await _contactService.GetByAreaCodeAsync(contact2.AreaCode);

            Assert.NotNull(foundRecords);
            Assert.NotEmpty(foundRecords);
        }

        [Fact]
        public async Task GetByAreaCodeNotFoundAsync()
        {
            var foundRecords = await _contactService.GetByAreaCodeAsync("101");
            Assert.Empty(foundRecords);
        }

        [Fact]
        public async Task GetByAreaCodeExceptionAsync()
        {
            var contact = ContactFixtures.CreateFakeContact(0);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetByAreaCodeAsync("ERROR"));
            Assert.Equal($"Some error occour when trying to get a contact by Area Code with Code: ERROR Contact.", exception.Message);
        }
    }
}
