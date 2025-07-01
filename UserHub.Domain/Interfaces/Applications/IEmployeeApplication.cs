using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications
{
    public interface IEmployeeApplication
    {
        Task AddEmployeeAsync(EmployeeCreateDto clientDto);
        Task UpdateEmployeeAsync(EmployeeUpdateDto clientDto);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();        
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto?> GetEmployeeByEmailAsync(string email);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesByRoleAsync(TypeRole role);
    }
}