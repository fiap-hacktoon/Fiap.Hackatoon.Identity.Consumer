using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee contact);
        Task UpdateAsync(Employee contact);
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllAsync();        
        Task<IEnumerable<Employee>> GetByEmailAsync(string email);
        Task DeleteAsync(int id);
        Task<IEnumerable<Employee>> GetByRoleAsync(TypeRole role);
    }
}
