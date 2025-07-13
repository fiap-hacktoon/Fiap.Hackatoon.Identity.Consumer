using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
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

        //[Fact]
        //public async Task GetClientsByAreaCodeAsyncRepositorySuccess()
        //{
        //    var areaCode = "11";
        //    var contactList = CommonTestData.GetClientListObject();

        //    _clientServiceMock.Setup(service => service.GetByAreaCodeAsync(areaCode)).ReturnsAsync(contactList);

        //    var result = await _clientApplication.GetClientsByAreaCodeAsync(areaCode);

        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count());
        //    _clientServiceMock.Verify(service => service.GetByAreaCodeAsync(areaCode), Times.Once);
        //}
    }
}
