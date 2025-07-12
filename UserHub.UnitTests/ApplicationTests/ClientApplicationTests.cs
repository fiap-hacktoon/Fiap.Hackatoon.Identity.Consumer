using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.UserHub.UnitTests.ApplicationTests
{
    public class ClientApplicationTests
    {
        private Mock<IClientService> _clientServiceMock;
        private Mock<ILogger<ClientApplication>> _loggerMock;
        private ClientApplication _clientApplication;
        private Mock<IElasticClient<Client>> _elasticClientMock;

        public ClientApplicationTests()
        {
            _clientServiceMock = new Mock<IClientService>();
            _loggerMock = new Mock<ILogger<ClientApplication>>();
            _elasticClientMock = new Mock<IElasticClient<Client>>();
            _clientApplication = new ClientApplication(_clientServiceMock.Object, _loggerMock.Object, _elasticClientMock.Object);
        }

        [Fact]
        public async Task AddClientAsync_ValidInput_CallsAddAndCreate()
        {
            var mockService = new Mock<IClientService>();
            var mockLogger = new Mock<ILogger<ClientApplication>>();
            var mockElastic = new Mock<IElasticClient<Client>>();
            var app = new ClientApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new ClientCreateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "New Client",
                Email = "client@example.com",
                Document = "11122233344",
                Password = "securePass",
                Birth = new DateOnly(1990, 1, 1)
            };

            await app.AddClientAsync(dto);

            mockService.Verify(s => s.AddAsync(It.IsAny<Client>()), Times.Once);
            mockElastic.Verify(e => e.Create(It.IsAny<Client>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddClientAsync_WhenServiceThrows_LogsAndThrows()
        {
            var mockService = new Mock<IClientService>();
            var mockLogger = new Mock<ILogger<ClientApplication>>();
            var mockElastic = new Mock<IElasticClient<Client>>();
            var app = new ClientApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new ClientCreateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "Fail Case",
                Email = "fail@example.com",
                Document = "00000000000",
                Password = "fail",
                Birth = new DateOnly(1990, 1, 1)
            };

            mockService.Setup(s => s.AddAsync(It.IsAny<Client>())).ThrowsAsync(new Exception("DB failure"));

            await Assert.ThrowsAsync<Exception>(() => app.AddClientAsync(dto));
            mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Ocorreu um erro")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateClientAsync_ValidClient_UpdatesSuccessfully()
        {
            var mockService = new Mock<IClientService>();
            var mockLogger = new Mock<ILogger<ClientApplication>>();
            var mockElastic = new Mock<IElasticClient<Client>>();
            var app = new ClientApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var existingClient = new Client{
                Id = 1,
                Name = "Old Name",
                Creation = DateTime.Now,
                Document = "11122233344",
                Email = "email@example.com",
                Password = "senha",
                TypeRole = (int)TypeRole.Client,
                Birth = new DateOnly(1990, 1, 1)
            };

            var dto = new ClientUpdateEvent
            {
                Id = 1,
                Name = "Updated Name",
                Email = "updated@example.com",
                Document = "99999999999",
                Birth = new DateOnly(1990, 1, 1)
            };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(existingClient);

            await app.UpdateClientAsync(dto);

            mockService.Verify(s => s.GetByIdAsync(dto.Id), Times.Once);
            mockService.Verify(s => s.UpdateAsync(It.Is<Client>(c => c.Name == dto.Name && c.Email == dto.Email)), Times.Once);
        }

        [Fact]
        public async Task UpdateClientAsync_WhenClientNotFound_ThrowsException()
        {
            var mockService = new Mock<IClientService>();
            var mockLogger = new Mock<ILogger<ClientApplication>>();
            var mockElastic = new Mock<IElasticClient<Client>>();
            var app = new ClientApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new ClientUpdateEvent { Id = 99 };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync((Client)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => app.UpdateClientAsync(dto));
        }

        [Fact]
        public async Task UpdateClientAsync_WhenUpdateThrows_LogsAndThrows()
        {
            var mockService = new Mock<IClientService>();
            var mockLogger = new Mock<ILogger<ClientApplication>>();
            var mockElastic = new Mock<IElasticClient<Client>>();
            var app = new ClientApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var existingClient = new Client
            {
                Id = 1,
                Name = "Old Name",
                Creation = DateTime.Now,
                Document = "11122233344",
                Email = "email@example.com",
                Password = "senha",
                TypeRole = (int)TypeRole.Client,
                Birth = new DateOnly(1990, 1, 1)
            };
            var dto = new ClientUpdateEvent
            {
                Id = 1,
                Name = "Updated",
                Email = "email",
                Document = "doc",
                Birth = new DateOnly(1990, 1, 1)
            };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(existingClient);
            mockService.Setup(s => s.UpdateAsync(It.IsAny<Client>())).ThrowsAsync(new Exception("Update failed"));

            await Assert.ThrowsAsync<Exception>(() => app.UpdateClientAsync(dto));
            mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Ocorreu um erro")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        #region GET

        [Fact]
        public async Task GetAllClientsAsyncRepositorySuccess()
        {
            var clients = CommonTestData.GetClientListObject();

            _clientServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(clients);

            var result = await _clientApplication.GetAllClientsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _clientServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllClientsAsyncRepositoryFail()
        {
            _clientServiceMock.Setup(service => service.GetAllAsync()).ThrowsAsync(new Exception("ERRO-SIMULADO"));

            var result = await _clientApplication.GetAllClientsAsync();

            Assert.Null(result);
            _clientServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
            _loggerMock.Equals("Ocorreu um erro na consulta de todos os contatos. Erro: ERRO-SIMULADO");
        }

        [Fact]
        public async Task GetContactByIdAsyncRepositorySuccess()
        {
            var clientId = 1;
            var client = CommonTestData.GetClientObject();
            _clientServiceMock.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync(client);

            var result = await _clientApplication.GetClientByIdAsync(clientId);

            Assert.NotNull(result);
            Assert.Equal(clientId, result.Id);
            _clientServiceMock.Verify(service => service.GetByIdAsync(client.Id), Times.Once);
        }

        [Fact]
        public async Task GetContactByIdAsyncRepositoryFail()
        {
            var clientId = 1;
            _clientServiceMock.Setup(service => service.GetByIdAsync(clientId)).ReturnsAsync((Client)null);

            var result = await _clientApplication.GetClientByIdAsync(clientId);

            Assert.Null(result);
            _clientServiceMock.Verify(service => service.GetByIdAsync(clientId), Times.Once);
        }

        #endregion END GET
    }
}
