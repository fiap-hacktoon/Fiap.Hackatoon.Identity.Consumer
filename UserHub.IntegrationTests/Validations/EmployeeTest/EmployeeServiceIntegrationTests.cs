using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using FIAP.TechChallenge.UserHub.Domain.Services;
using FIAP.TechChallenge.UserHub.Infrastructure.Repositories;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.EmployeeTest
{
    public class EmployeeServiceIntegrationTests : BaseServiceTests
    {
        private readonly IEmployeeService _contactService;
        private readonly IEmployeeService _contactServiceException;
        private readonly IEmployeeRepository _contactRepository;
        private Mock<ILogger<EmployeeService>> _loggerMock;
        public readonly Random RandomId;

        public EmployeeServiceIntegrationTests()
        {
            _contactRepository = new EmployeeRepository(_context);
            _loggerMock = new Mock<ILogger<EmployeeService>>();
            _contactService = new EmployeeService(_contactRepository, _loggerMock.Object);
            _contactServiceException = new EmployeeService(null, _loggerMock.Object);
            RandomId = new Random();
        }

        [Fact]
        public async Task GetByIdContactSuccessAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

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
            var contact = EmployeeFixtures.CreateFakeContact(0);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetByIdAsync(contact.Id));
            Assert.Equal($"Some error occour when trying to get a contact with Id: {contact.Id} Contact.", exception.Message);
        }

        [Fact]
        public async Task GetAllContactSuccessAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

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
            var contact = EmployeeFixtures.CreateFakeContact(0);
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetAllAsync());
            Assert.Equal($"Some error occour when trying to get all contacts in database.", exception.Message);
        }

        //[Fact]
        //public async Task GetByAreaCodeSuccessAsync()
        //{
        //    var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

        //    await _context.AddRangeAsync(contact1, contact2, contact3);

        //    await SaveChanges();

        //    var foundRecords = await _contactService.GetByAreaCodeAsync(contact2.AreaCode);

        //    Assert.NotNull(foundRecords);
        //    Assert.NotEmpty(foundRecords);
        //}

        //[Fact]
        //public async Task GetByAreaCodeNotFoundAsync()
        //{
        //    var foundRecords = await _contactService.GetByAreaCodeAsync("101");
        //    Assert.Empty(foundRecords);
        //}

        //[Fact]
        //public async Task GetByAreaCodeExceptionAsync()
        //{
        //    var contact = EmployeeFixtures.CreateFakeContact(0);
        //    var exception = await Assert.ThrowsAsync<Exception>(async () => await _contactServiceException.GetByAreaCodeAsync("ERROR"));
        //    Assert.Equal($"Some error occour when trying to get a contact by Area Code with Code: ERROR Contact.", exception.Message);
        //}
    }
}
