using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetEmailCodeAsync(string email);
        Task<IEnumerable<Employee>> GetByRoleAsync(TypeRole role);
    }
}