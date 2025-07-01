using FIAP.TechChallenge.UserHub.Application.Applications;
using FIAP.TechChallenge.UserHub.Domain.Entities;
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

        public EmployeeApplicationTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _loggerMock = new Mock<ILogger<EmployeeApplication>>();
            _employeeApplication = new EmployeeApplication(_employeeServiceMock.Object, _loggerMock.Object);
        }

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

        //[Fact]
        //public async Task GetEmployeesByAreaCodeAsyncRepositorySuccess()
        //{
        //    var areaCode = "11";
        //    var EmployeeList = CommonTestData.GetEmployeeListObject();

        //    _clientServiceMock.Setup(service => service.GetByAreaCodeAsync(areaCode)).ReturnsAsync(EmployeeList);

        //    var result = await _clientApplication.GetEmployeesByAreaCodeAsync(areaCode);

        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count());
        //    _clientServiceMock.Verify(service => service.GetByAreaCodeAsync(areaCode), Times.Once);
        //}
    }
}
