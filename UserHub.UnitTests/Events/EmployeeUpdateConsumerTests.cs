using System;
using System.Threading.Tasks;
using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;
using Moq;
using Xunit;

namespace FIAP.TechChallenge.UserHub.UnitTests.Events
{
    public class EmployeeUpdateConsumerTests
    {
        [Fact]
        public async Task Consume_ValidEvent_CallsUpdateEmployeeAsync()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeUpdateConsumer(mockService.Object);

            var evt = new EmployeeUpdateEvent
            {
                TypeRole = TypeRole.Kitchen,
                Name = "Updated Emp",
                Email = "updated@emp.com"
            };

            var context = new Mock<ConsumeContext<EmployeeUpdateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            await consumer.Consume(context.Object);

            mockService.Verify(x => x.UpdateEmployeeAsync(evt), Times.Once);
        }

        [Fact]
        public async Task Consume_WhenUpdateEmployeeThrows_ExceptionIsPropagated()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeUpdateConsumer(mockService.Object);

            var evt = new EmployeeUpdateEvent
            {
                TypeRole = TypeRole.Manager,
                Name = "Fail Emp",
                Email = "fail@emp.com"
            };

            var context = new Mock<ConsumeContext<EmployeeUpdateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            mockService.Setup(s => s.UpdateEmployeeAsync(It.IsAny<EmployeeUpdateEvent>()))
                       .ThrowsAsync(new Exception("Simulated failure"));

            await Assert.ThrowsAsync<Exception>(() => consumer.Consume(context.Object));
        }

        [Fact]
        public async Task Consume_NullMessage_DoesNotCallUpdateEmployeeAsync()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeUpdateConsumer(mockService.Object);

            var context = new Mock<ConsumeContext<EmployeeUpdateEvent>>();
            context.Setup(x => x.Message).Returns((EmployeeUpdateEvent)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(context.Object));

            mockService.Verify(x => x.UpdateEmployeeAsync(It.IsAny<EmployeeUpdateEvent>()), Times.Never);
        }
    }
}
