using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.UserHub.Application.Applications
{
    public class EmployeeApplication(IEmployeeService employeeService, ILogger<EmployeeApplication> logger) : IEmployeeApplication
    {
        private readonly IEmployeeService _employeeService = employeeService;

        private readonly ILogger<EmployeeApplication> _logger = logger;

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            try
            {
                var contacts = await _employeeService.GetAllAsync();
                var contactDtos = new List<EmployeeDto>();

                foreach (var contact in contacts)
                    contactDtos.Add(new EmployeeDto
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Email = contact.Email,
                        TypeRole = (TypeRole)contact.TypeRole
                    });

                return contactDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta de todos os funcionários. Erro: {e.Message}");
                return null;
            }
        }        

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var contact = await _employeeService.GetByIdAsync(id);
                if (contact == null)
                    return null;

                return new EmployeeDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Email = contact.Email,
                    TypeRole = (TypeRole)contact.TypeRole
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta dos funcionários por Id. Erro: {e.Message}");
                return null;
            }
        }
        
        public async Task<EmployeeDto?> GetEmployeeByEmailAsync(string email)
        {
            try
            {
                var contact = await _employeeService.GetEmailCodeAsync(email);
                if (contact == null)
                    return null;

                return new EmployeeDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Email = contact.Email,
                    TypeRole = (TypeRole)contact.TypeRole
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta dos funcionários por Email. Erro: {e.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesByRoleAsync(TypeRole role)
        {
            try
            {
                var employee = await _employeeService.GetByRoleAsync(role);
                var employeeDtos = new List<EmployeeDto>();

                foreach (var contact in employee)
                    employeeDtos.Add(new EmployeeDto
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Email = contact.Email,
                        TypeRole = (TypeRole)contact.TypeRole
                    });

                return employeeDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta dos funcionários por cargo. Erro: {e.Message}");
                return null;
            }
        }

        public async Task AddEmployeeAsync(EmployeeCreateDto employeeDto)
        {
            try
            {
                var employee = new Employee
                {
                    Creation = DateTime.Now,
                    Email = employeeDto.Email,
                    Name = employeeDto.Name,
                    Password = employeeDto.Password,
                    TypeRole = (int)employeeDto.TypeRole
                };

                await _employeeService.AddEmployeeAsync(employee);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta de todos os funcionários. Erro: {e.Message}");
            }
        }

        public async Task UpdateEmployeeAsync(EmployeeUpdateDto employeeDto)
        {
            try
            {
                var employee = new Employee
                {
                    Creation = DateTime.Now,
                    Email = employeeDto.Email,
                    Name = employeeDto.Name,
                    TypeRole = (int)employeeDto.TypeRole
                };

                await _employeeService.UpdateEmployeeAsync(employee);
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro ao atualizar os funcionários. Erro: {e.Message}");
            }
        }
    }
}
