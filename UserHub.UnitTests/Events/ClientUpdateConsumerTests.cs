using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;
using Moq;

namespace FIAP.TechChallenge.UserHub.UnitTests.Events
{
    public class ClientUpdateConsumerTests
    {
        [Fact]
        public async Task Consume_ValidEvent_CallsUpdateClientAsync()
        {
            var mockService = new Mock<IClientApplication>();
            var consumer = new ClientUpdateConsumer(mockService.Object);

            var evt = new ClientUpdateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "Updated Client",
                Email = "update@example.com",
                Document = "12345678900",
                Birth = new DateOnly(1985, 5, 15)
            };

            var context = new Mock<ConsumeContext<ClientUpdateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            await consumer.Consume(context.Object);

            mockService.Verify(x => x.UpdateClientAsync(evt), Times.Once);
        }

        [Fact]
        public async Task Consume_WhenUpdateClientThrows_ExceptionIsPropagated()
        {
            var mockService = new Mock<IClientApplication>();
            var consumer = new ClientUpdateConsumer(mockService.Object);

            var evt = new ClientUpdateEvent
            {
                TypeRole = TypeRole.Client,
                Name = "Fail Test",
                Email = "fail@example.com",
                Document = "00000000000",                
                Birth = new DateOnly(1985, 5, 15)
            };

            var context = new Mock<ConsumeContext<ClientUpdateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            mockService.Setup(s => s.UpdateClientAsync(It.IsAny<ClientUpdateEvent>()))
                       .ThrowsAsync(new Exception("Simulated failure"));

            await Assert.ThrowsAsync<Exception>(() => consumer.Consume(context.Object));
        }

        [Fact]
        public async Task Consume_NullMessage_DoesNotCallUpdateClientAsync()
        {
            var mockService = new Mock<IClientApplication>();
            var consumer = new ClientUpdateConsumer(mockService.Object);

            var context = new Mock<ConsumeContext<ClientUpdateEvent>>();
            context.Setup(x => x.Message).Returns((ClientUpdateEvent)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(context.Object));

            mockService.Verify(x => x.UpdateClientAsync(It.IsAny<ClientUpdateEvent>()), Times.Never);
        }
    }
}
