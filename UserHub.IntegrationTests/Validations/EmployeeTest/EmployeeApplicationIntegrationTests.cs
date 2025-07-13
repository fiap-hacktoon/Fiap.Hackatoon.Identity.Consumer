using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using FIAP.TechChallenge.UserHub.Domain.Services;
using FIAP.TechChallenge.UserHub.Infrastructure.Data;
using FIAP.TechChallenge.UserHub.Infrastructure.Repositories;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        public async Task EmployeeAddConsumer_Should_Consume_EmployeeCreateEvent()
        {
            await using var provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<EmployeeAddConsumer>();
                    cfg.AddConsumerTestHarness<EmployeeAddConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            try
            {
                var bus = provider.GetRequiredService<IBus>();

                var testEvent = new EmployeeCreateEvent
                {
                    Name = "Integration Test",
                    Email = "test@employee.com",
                    Password = "123456",
                    TypeRole = TypeRole.Kitchen
                };

                await bus.Send(testEvent);

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<EmployeeAddConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<EmployeeCreateEvent>());
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task ShouldInsertEmployee()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ContactsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ContactsDbContext(dbContextOptions);
            var repository = new EmployeeRepository(context);
            var serviceLogger = new Mock<ILogger<EmployeeService>>().Object;
            var appLogger = new Mock<ILogger<EmployeeApplication>>().Object;
            var elastic = new Mock<IElasticClient<Employee>>().Object;

            var employeeService = new EmployeeService(repository, serviceLogger);
            var employeeApp = new EmployeeApplication(employeeService, appLogger, elastic);

            await using var provider = new ServiceCollection()
                .AddSingleton<IEmployeeApplication>(employeeApp)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<EmployeeAddConsumer>();
                    cfg.AddConsumerTestHarness<EmployeeAddConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            try
            {
                var bus = provider.GetRequiredService<IBus>();

                var testEvent = new EmployeeCreateEvent
                {
                    Name = "New Employee",
                    Email = "new@emp.com",
                    Password = "123456",
                    TypeRole = TypeRole.Kitchen
                };

                await bus.Send(testEvent);

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<EmployeeAddConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<EmployeeCreateEvent>());

                var created = await context.Employees.FirstOrDefaultAsync(e => e.Email == "new@emp.com");
                Assert.NotNull(created);
                Assert.Equal("New Employee", created!.Name);
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task ShouldUpdateEmployee()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ContactsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ContactsDbContext(dbContextOptions);
            var repository = new EmployeeRepository(context);
            var serviceLogger = new Mock<ILogger<EmployeeService>>().Object;
            var appLogger = new Mock<ILogger<EmployeeApplication>>().Object;
            var elastic = new Mock<IElasticClient<Employee>>().Object;

            var employeeService = new EmployeeService(repository, serviceLogger);
            var employeeApp = new EmployeeApplication(employeeService, appLogger, elastic);

            await context.Employees.AddAsync(new Employee
            {
                Name = "Old Name",
                Email = "old@emp.com",
                Password = "123456",
                TypeRole = (int)TypeRole.Kitchen,
                Creation = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var existing = await context.Employees.FirstOrDefaultAsync(e => e.Email == "old@emp.com");
            Assert.NotNull(existing);

            await using var provider = new ServiceCollection()
                .AddSingleton<IEmployeeApplication>(employeeApp)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<EmployeeUpdateConsumer>();
                    cfg.AddConsumerTestHarness<EmployeeUpdateConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            try
            {
                var bus = provider.GetRequiredService<IBus>();

                var updateEvent = new EmployeeUpdateEvent
                {
                    Id = existing!.Id,
                    Name = "Updated Name",
                    Email = "updated@emp.com",
                    TypeRole = TypeRole.Attendant
                };

                await bus.Send(updateEvent);

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<EmployeeUpdateConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<EmployeeUpdateEvent>());

                var updated = await context.Employees.FirstOrDefaultAsync(e => e.Id == existing.Id);
                Assert.Equal("Updated Name", updated!.Name);
                Assert.Equal("updated@emp.com", updated.Email);
            }
            finally
            {
                await harness.Stop();
            }
        }

        #region GET
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
        #endregion

    }
}
