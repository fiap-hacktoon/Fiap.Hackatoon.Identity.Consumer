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

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations.ClientTest
{
    public class ClientApplicationIntegrationTests : BaseServiceTests
    {
        private readonly IClientService _contactService;
        private readonly IClientApplication _contactApplicationException;
        private readonly IClientApplication _contactApplication;
        private readonly IClientRepository _contactRepository;
        private Mock<ILogger<ClientService>> _loggerServiceMock;
        private Mock<ILogger<ClientApplication>> _loggerApplicationMock;
        public readonly Random RandomId;
        private Mock<IElasticClient<Client>> _elasticClientMock;

        public ClientApplicationIntegrationTests()
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
        public async Task ClientAddConsumer_Should_Consume_ClientCreateEvent()
        {
            var clientAppMock = new Mock<IClientApplication>();

            await using var provider = new ServiceCollection()
                .AddSingleton(clientAppMock.Object)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<ClientAddConsumer>();
                    cfg.AddConsumerTestHarness<ClientAddConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();

            await harness.Start();
            try
            {
                var testEvent = new ClientCreateEvent
                {
                    Name = "Integration Test",
                    Email = "integration@test.com",
                    Document = "12345678900",
                    Password = "test123",
                    TypeRole = TypeRole.Client,
                    Birth = new DateOnly(1990, 1, 1)
                };

                var bus = provider.GetRequiredService<IBus>();
                await bus.Send(testEvent);

                Assert.True(await harness.Consumed.Any<ClientCreateEvent>(), "Event not consumed at queue level");

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<ClientAddConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<ClientCreateEvent>(), "Event not consumed by ClientAddConsumer");

                clientAppMock.Verify(app => app.AddClientAsync(It.Is<ClientCreateEvent>(e => e.Email == testEvent.Email)), Times.Once);
            }
            finally
            {
                await harness.Stop();
            }
        }

        /// <summary>
        /// ClientCreateEvent_Should_Persist_Client_ToDatabase
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldInsertClient()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ContactsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ContactsDbContext(dbContextOptions);
            var repository = new ClientRepository(context);
            var serviceLogger = new Mock<ILogger<ClientService>>().Object;
            var appLogger = new Mock<ILogger<ClientApplication>>().Object;
            var elastic = new Mock<IElasticClient<Client>>().Object;

            var clientService = new ClientService(repository, serviceLogger);
            var clientApp = new ClientApplication(clientService, appLogger, elastic);

            await using var provider = new ServiceCollection()
                .AddSingleton<IClientApplication>(clientApp)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<ClientAddConsumer>();
                    cfg.AddConsumerTestHarness<ClientAddConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            try
            {
                var bus = provider.GetRequiredService<IBus>();

                var testEvent = new ClientCreateEvent
                {
                    Name = "Persisted Client",
                    Email = "real@save.com",
                    Document = "22233344455",
                    Password = "test123",
                    TypeRole = TypeRole.Client,
                    Birth = new DateOnly(1991, 1, 1)
                };

                await bus.Send(testEvent);

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<ClientAddConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<ClientCreateEvent>());

                var saved = await context.Clients.ToListAsync();
                Assert.Contains(saved, c => c.Email == "real@save.com");
            }
            finally
            {
                await harness.Stop();
            }
        }

        /// <summary>
        /// ClientCreateEvent_Should_Persist_Client_ToDatabase
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldUpdateClient()
        {
            var dbName = Guid.NewGuid().ToString();

            var dbContextOptions = new DbContextOptionsBuilder<ContactsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ContactsDbContext(dbContextOptions);
            var repository = new ClientRepository(context);
            var serviceLogger = new Mock<ILogger<ClientService>>().Object;
            var appLogger = new Mock<ILogger<ClientApplication>>().Object;
            var elastic = new Mock<IElasticClient<Client>>().Object;

            var clientService = new ClientService(repository, serviceLogger);
            var clientApp = new ClientApplication(clientService, appLogger, elastic);

            await using var provider = new ServiceCollection()
                .AddSingleton<IClientApplication>(clientApp)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<ClientAddConsumer>();
                    cfg.AddConsumer<ClientUpdateConsumer>();
                    cfg.AddConsumerTestHarness<ClientAddConsumer>();
                    cfg.AddConsumerTestHarness<ClientUpdateConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            try
            {
                var bus = provider.GetRequiredService<IBus>();

                var testEvent = new ClientCreateEvent
                {
                    Name = "Persisted Client",
                    Email = "real@save.com",
                    Document = "22233344455",
                    Password = "test123",
                    TypeRole = TypeRole.Client,
                    Birth = new DateOnly(1991, 1, 1)
                };

                await bus.Send(testEvent);

                var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<ClientAddConsumer>>();
                Assert.True(await consumerHarness.Consumed.Any<ClientCreateEvent>());

                var created = await context.Clients.FirstOrDefaultAsync(c => c.Email == "real@save.com");
                Assert.NotNull(created);

                // Enviar evento de update
                var updateEvent = new ClientUpdateEvent
                {
                    Id = created!.Id,
                    Name = "Updated Name",
                    Email = "updated@save.com",
                    Document = created.Document,
                    Birth = created.Birth
                };

                await bus.Send(updateEvent);

                var updateHarness = provider.GetRequiredService<IConsumerTestHarness<ClientUpdateConsumer>>();
                Assert.True(await updateHarness.Consumed.Any<ClientUpdateEvent>());

                var updated = await context.Clients.FirstOrDefaultAsync(c => c.Id == created.Id);
                Assert.Equal("Updated Name", updated!.Name);
                Assert.Equal("updated@save.com", updated.Email);
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

        #endregion END GET
    }
}
