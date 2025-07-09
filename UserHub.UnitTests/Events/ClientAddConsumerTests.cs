using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;
using Moq;

namespace FIAP.TechChallenge.UserHub.UnitTests.Events
{
    public class ClientAddConsumerTests
    {
        [Fact]
        public async Task Consume_ValidEvent_CallsAddClientAsync()
        {
            // Arrange
            var mockClientService = new Mock<IClientApplication>();
            var consumer = new ClientAddConsumer(mockClientService.Object);

            var testEvent = new ClientCreateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "Test Client",
                Email = "test@example.com",
                Document = "12345678900",
                Password = "password123",
                Birth = new DateOnly(1990, 1, 1)
            };

            var mockContext = new Mock<ConsumeContext<ClientCreateEvent>>();
            mockContext.Setup(x => x.Message).Returns(testEvent);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockClientService.Verify(x => x.AddClientAsync(testEvent), Times.Once);
        }

        [Fact]
        public async Task Consume_WhenAddClientThrows_ExceptionIsPropagated()
        {
            // Arrange
            var mockClientService = new Mock<IClientApplication>();
            var consumer = new ClientAddConsumer(mockClientService.Object);

            var testEvent = new ClientCreateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "Error Test",
                Email = "fail@example.com",
                Document = "00000000000",
                Password = "error",
                Birth = new DateOnly(1990, 1, 1)
            };

            var mockContext = new Mock<ConsumeContext<ClientCreateEvent>>();
            mockContext.Setup(x => x.Message).Returns(testEvent);

            mockClientService
                .Setup(s => s.AddClientAsync(It.IsAny<ClientCreateEvent>()))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => consumer.Consume(mockContext.Object));
        }

        [Fact]
        public async Task Consume_NullMessage_DoesNotCallAddClientAsync()
        {
            var mockClientService = new Mock<IClientApplication>();
            var consumer = new ClientAddConsumer(mockClientService.Object);

            var mockContext = new Mock<ConsumeContext<ClientCreateEvent>>();
            mockContext.Setup(x => x.Message).Returns((ClientCreateEvent)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(mockContext.Object));

            mockClientService.Verify(x => x.AddClientAsync(It.IsAny<ClientCreateEvent>()), Times.Never);
        }
    }
}
