using FIAP.TechChallenge.UserHub.Domain.Entities;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task AddAsync(Client contact);
        Task UpdateAsync(Client contact);
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();        
        Task<IEnumerable<Client>> GetByEmailAsync(string email);
        Task DeleteAsync(int id);
    }
}
