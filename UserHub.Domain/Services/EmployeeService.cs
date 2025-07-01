using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.UserHub.Domain.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<EmployeeService> _logger = logger;

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                return await _employeeRepository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                var message = $"Some error occour when trying to get a employee with Id: {id} employee.";
                _logger.LogError(message);
                throw new Exception(message);
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _employeeRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to get all employees in database.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }

        public async Task<Employee> GetEmailCodeAsync(string email)
        {
            try
            {
                var employeeEmailList = await _employeeRepository.GetByEmailAsync(email);
                return employeeEmailList != null && employeeEmailList.Any() ? 
                       employeeEmailList.FirstOrDefault() : 
                       null;
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to get a employee by Area Code with Email: {email} employee.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }

        public async Task<IEnumerable<Employee>> GetByRoleAsync(TypeRole role)
        {
            try
            {
                return await _employeeRepository.GetByRoleAsync(role);
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to get a employee by Role with : {role} employee.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }
    }
}