using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications
{
    public interface IClientApplication
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto?> GetClientByIdAsync(int id);
        Task<ClientDto?> GetClientByEmailAsync(string email);
    }
}