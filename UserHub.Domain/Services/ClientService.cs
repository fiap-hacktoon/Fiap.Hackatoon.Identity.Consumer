using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.UserHub.Domain.Services
{
    public class ClientService(IClientRepository contactRepository, ILogger<ClientService> logger) : IClientService
    {
        private readonly IClientRepository _contactRepository = contactRepository;
        private readonly ILogger<ClientService> _logger = logger;

        public async Task AddAsync(Client contact)
        {
            try
            {
                await _contactRepository.AddAsync(contact);
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to add contacts in database.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }

        public async Task UpdateAsync(Client contact)
        {
            try
            {
                await _contactRepository.UpdateAsync(contact);
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to update contacts in database.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            try
            {
                return await _contactRepository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                var message = $"Some error occour when trying to get a contact with Id: {id} Contact.";
                _logger.LogError(message);
                throw new Exception(message);
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            try
            {
                return await _contactRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to get all contacts in database.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }

        public async Task<Client> GetEmailCodeAsync(string email)
        {
            try
            {
                var contactEmailList = await _contactRepository.GetByEmailAsync(email);
                return contactEmailList != null && contactEmailList.Any() ? 
                       contactEmailList.FirstOrDefault() : 
                       null;
            }
            catch (Exception e)
            {
                var message = $"Some error occour when trying to get a contact by Area Code with Email: {email} Contact.";
                _logger.LogError(message, e);
                throw new Exception(message);
            }
        }        
    }
}