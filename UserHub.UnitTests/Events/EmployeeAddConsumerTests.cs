using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Api.Events;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;
using Moq;

namespace FIAP.TechChallenge.UserHub.UnitTests.Events
{
    public class EmployeeAddConsumerTests
    {
        [Fact]
        public async Task Consume_ValidEvent_CallsAddEmployeeAsync()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeAddConsumer(mockService.Object);

            var evt = new EmployeeCreateEvent
            {
                TypeRole = TypeRole.Employee,
                Name = "New Employee",
                Email = "employee@example.com",
                Document = "98765432100",
                Password = "emp123",
                Birth = new DateOnly(1992, 3, 10)
            };

            var context = new Mock<ConsumeContext<EmployeeCreateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            await consumer.Consume(context.Object);

            mockService.Verify(x => x.AddEmployeeAsync(evt), Times.Once);
        }

        [Fact]
        public async Task Consume_WhenAddEmployeeThrows_ExceptionIsPropagated()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeAddConsumer(mockService.Object);

            var evt = new EmployeeCreateEvent
            {
                TypeRole = TypeRole.Employee,
                Name = "Fail Emp",
                Email = "fail@emp.com",
                Document = "00000000000",
                Password = "fail",
                Birth = new DateOnly(1992, 3, 10)
            };

            var context = new Mock<ConsumeContext<EmployeeCreateEvent>>();
            context.Setup(x => x.Message).Returns(evt);

            mockService.Setup(s => s.AddEmployeeAsync(It.IsAny<EmployeeCreateEvent>()))
                       .ThrowsAsync(new Exception("Simulated failure"));

            await Assert.ThrowsAsync<Exception>(() => consumer.Consume(context.Object));
        }

        [Fact]
        public async Task Consume_NullMessage_DoesNotCallAddEmployeeAsync()
        {
            var mockService = new Mock<IEmployeeApplication>();
            var consumer = new EmployeeAddConsumer(mockService.Object);

            var context = new Mock<ConsumeContext<EmployeeCreateEvent>>();
            context.Setup(x => x.Message).Returns((EmployeeCreateEvent)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(context.Object));

            mockService.Verify(x => x.AddEmployeeAsync(It.IsAny<EmployeeCreateEvent>()), Times.Never);
        }
    }
}
