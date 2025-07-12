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

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.EmployeeTest
{
    public class EmployeeApplicationIntegrationTests : BaseServiceTests
    {
        private readonly IEmployeeService _contactService;
        private readonly IEmployeeApplication _contactApplicationException;
        private readonly IEmployeeApplication _contactApplication;
        private readonly IEmployeeRepository _contactRepository;
        private Mock<ILogger<EmployeeService>> _loggerServiceMock;
        private Mock<ILogger<EmployeeApplication>> _loggerApplicationMock;
        public readonly Random RandomId;
        private Mock<IElasticClient<Employee>> _elasticEmployeeMock;

        public EmployeeApplicationIntegrationTests()
        {
            _contactRepository = new EmployeeRepository(_context);
            _loggerServiceMock = new Mock<ILogger<EmployeeService>>();
            _loggerApplicationMock = new Mock<ILogger<EmployeeApplication>>();
            _contactService = new EmployeeService(_contactRepository, _loggerServiceMock.Object);
            _elasticEmployeeMock = new Mock<IElasticClient<Employee>>();
            _contactApplication = new EmployeeApplication(_contactService, _loggerApplicationMock.Object, _elasticEmployeeMock.Object);
            _contactApplicationException = new EmployeeApplication(null, _loggerApplicationMock.Object, _elasticEmployeeMock.Object);
            RandomId = new Random();
        }

        [Fact]
        public async Task GetAllContactSuccessAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplication.GetAllEmployeesAsync();
            Assert.NotNull(contactList);
            Assert.NotEmpty(contactList);
        }

        [Fact]
        public async Task GetAllContactExceptionAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplicationException.GetAllEmployeesAsync();
            Assert.Null(contactList);
        }

        [Fact]
        public async Task GetContactByIdExceptionAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplicationException.GetEmployeeByIdAsync(contact2.Id);
            Assert.Null(contactList);
        }

        [Fact]
        public async Task GetContactByIdSuccessAsync()
        {
            var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
            var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

            await _context.AddRangeAsync(contact1, contact2, contact3);

            await SaveChanges();

            var contactList = await _contactApplication.GetEmployeeByIdAsync(contact2.Id);
            Assert.NotNull(contactList);
            Assert.Equal(contactList.Id, contact2.Id);
        }

        //[Fact]
        //public async Task GetContactByAreaCodeExceptionAsync()
        //{
        //    var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

        //    await _context.AddRangeAsync(contact1, contact2, contact3);

        //    await SaveChanges();

        //    var contactList = await _contactApplicationException.GetContactsByAreaCodeAsync(contact2.AreaCode);
        //    Assert.Null(contactList);
        //}

        //[Fact]
        //public async Task GetContactByAreaCodeSuccessAsync()
        //{
        //    var contact1 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact2 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));
        //    var contact3 = EmployeeFixtures.CreateFakeContact(RandomId.Next(999999999));

        //    await _context.AddRangeAsync(contact1, contact2, contact3);

        //    await SaveChanges();

        //    var contactList = await _contactApplication.GetContactsByAreaCodeAsync(contact2.AreaCode);
        //    Assert.NotNull(contactList);
        //    Assert.NotEmpty(contactList);
        //}
    }
}
