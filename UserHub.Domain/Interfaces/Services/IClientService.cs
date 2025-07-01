using FIAP.TechChallenge.UserHub.Domain.Entities;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Services
{
    public interface IClientService
    {
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> GetEmailCodeAsync(string email);
    }
}