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
    public class EmployeeApplicationTests
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private Mock<ILogger<EmployeeApplication>> _loggerMock;
        private EmployeeApplication _employeeApplication;
        private Mock<IElasticClient<Employee>> _elasticEmployeeMock;

        public EmployeeApplicationTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _loggerMock = new Mock<ILogger<EmployeeApplication>>();
            _elasticEmployeeMock = new Mock<IElasticClient<Employee>>();
            _employeeApplication = new EmployeeApplication(_employeeServiceMock.Object, _loggerMock.Object, _elasticEmployeeMock.Object);
        }

        [Fact]
        public async Task AddEmployeeAsync_ValidInput_CallsAddAndCreate()
        {
            var mockService = new Mock<IEmployeeService>();
            var mockLogger = new Mock<ILogger<EmployeeApplication>>();
            var mockElastic = new Mock<IElasticClient<Employee>>();
            var app = new EmployeeApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new EmployeeCreateEvent
            {
                TypeRole = TypeRole.Kitchen,
                Name = "New Emp",
                Email = "emp@example.com",                
                Password = "emp123"
            };

            await app.AddEmployeeAsync(dto);

            mockService.Verify(s => s.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
            mockElastic.Verify(e => e.Create(It.IsAny<Employee>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeAsync_WhenServiceThrows_LogsAndThrows()
        {
            var mockService = new Mock<IEmployeeService>();
            var mockLogger = new Mock<ILogger<EmployeeApplication>>();
            var mockElastic = new Mock<IElasticClient<Employee>>();
            var app = new EmployeeApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new EmployeeCreateEvent
            {
                TypeRole = TypeRole.Attendant,
                Name = "Error",
                Email = "error@emp.com",                
                Password = "error"
            };

            mockService.Setup(s => s.AddEmployeeAsync(It.IsAny<Employee>())).ThrowsAsync(new Exception("Insert error"));

            await Assert.ThrowsAsync<Exception>(() => app.AddEmployeeAsync(dto));
            mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Ocorreu um erro")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ValidEmployee_UpdatesSuccessfully()
        {
            var mockService = new Mock<IEmployeeService>();
            var mockLogger = new Mock<ILogger<EmployeeApplication>>();
            var mockElastic = new Mock<IElasticClient<Employee>>();
            var app = new EmployeeApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var existing = new Employee { Id = 1, Name = "Old", TypeRole = (int)TypeRole.Client, Creation = DateTime.Now, Email = "email@example.com" };
            var dto = new EmployeeUpdateEvent
            {
                Id = 1,
                Name = "Updated",
                Email = "updated@emp.com",
                TypeRole = TypeRole.Client
            };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(existing);

            await app.UpdateEmployeeAsync(dto);

            mockService.Verify(s => s.GetByIdAsync(dto.Id), Times.Once);
            mockService.Verify(s => s.UpdateEmployeeAsync(It.Is<Employee>(e => e.Name == dto.Name && e.Email == dto.Email)), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenNotFound_ThrowsException()
        {
            var mockService = new Mock<IEmployeeService>();
            var mockLogger = new Mock<ILogger<EmployeeApplication>>();
            var mockElastic = new Mock<IElasticClient<Employee>>();
            var app = new EmployeeApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var dto = new EmployeeUpdateEvent { Id = 99 };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync((Employee)null!);

            await Assert.ThrowsAsync<NullReferenceException>(() => app.UpdateEmployeeAsync(dto));
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenUpdateThrows_LogsAndThrows()
        {
            var mockService = new Mock<IEmployeeService>();
            var mockLogger = new Mock<ILogger<EmployeeApplication>>();
            var mockElastic = new Mock<IElasticClient<Employee>>();
            var app = new EmployeeApplication(mockService.Object, mockLogger.Object, mockElastic.Object);

            var existing = new Employee { Id = 1, Name = "Old", TypeRole = (int)TypeRole.Client, Creation = DateTime.Now, Email = "email@example.com" };
            var dto = new EmployeeUpdateEvent
            {
                Id = 1,
                Name = "TryUpdate",
                Email = "try@emp.com"
            };

            mockService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            mockService.Setup(s => s.UpdateEmployeeAsync(It.IsAny<Employee>())).ThrowsAsync(new Exception("Update failed"));

            await Assert.ThrowsAsync<Exception>(() => app.UpdateEmployeeAsync(dto));
            mockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Ocorreu um erro")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        #region GET
        [Fact]
        public async Task GetAllEmployeesAsyncRepositorySuccess()
        {
            var employees = CommonTestData.GetEmployeeListObject();

            _employeeServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(employees);

            var result = await _employeeApplication.GetAllEmployeesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _employeeServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployeesAsyncRepositoryFail()
        {
            _employeeServiceMock.Setup(service => service.GetAllAsync()).ThrowsAsync(new Exception("ERRO-SIMULADO"));

            var result = await _employeeApplication.GetAllEmployeesAsync();

            Assert.Null(result);
            _employeeServiceMock.Verify(service => service.GetAllAsync(), Times.Once);
            _loggerMock.Equals("Ocorreu um erro na consulta de todos os contatos. Erro: ERRO-SIMULADO");
        }

        [Fact]
        public async Task GetEmployeeByIdAsyncRepositorySuccess()
        {
            var employeeId = 1;
            var employee = CommonTestData.GetEmployeeObject();
            _employeeServiceMock.Setup(service => service.GetByIdAsync(employeeId)).ReturnsAsync(employee);

            var result = await _employeeApplication.GetEmployeeByIdAsync(employeeId);

            Assert.NotNull(result);
            Assert.Equal(employeeId, result.Id);
            _employeeServiceMock.Verify(service => service.GetByIdAsync(employee.Id), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsyncRepositoryFail()
        {
            var employeeId = 1;
            _employeeServiceMock.Setup(service => service.GetByIdAsync(employeeId)).ReturnsAsync((Employee)null);

            var result = await _employeeApplication.GetEmployeeByIdAsync(employeeId);

            Assert.Null(result);
            _employeeServiceMock.Verify(service => service.GetByIdAsync(employeeId), Times.Once);
        }         
        #endregion END GET
    }
}
