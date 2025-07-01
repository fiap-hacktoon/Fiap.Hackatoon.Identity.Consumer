using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.UserHub.Application.Applications
{
    public class ClientApplication(IClientService contactService, ILogger<ClientApplication> logger) : IClientApplication
    {
        private readonly IClientService _contactService = contactService;

        private readonly ILogger<ClientApplication> _logger = logger;

        public async Task AddClientAsync(ClientCreateDto clientDto)
        {
            try
            {
                var client = new Client
                {
                    Name = clientDto.Name,
                    Creation = DateTime.Now,
                    Document = clientDto.Document,
                    Email = clientDto.Email,
                    Password = clientDto.Password,
                    TypeRole = (int)clientDto.TypeRole
                };

                await _contactService.AddAsync(client);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro ao adicionar o cliente. Erro: {e.Message}");
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateClientAsync(ClientUpdateDto clientDto)
        {
            try
            {
                var client = new Client
                {
                    Name = clientDto.Name,
                    Creation = DateTime.Now,
                    Document = clientDto.Document,
                    Email = clientDto.Email,
                    TypeRole = (int)clientDto.TypeRole
                };

                await _contactService.UpdateAsync(client);

            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro ao atualizar o cliente. Erro: {e.Message}");
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            try
            {
                var contacts = await _contactService.GetAllAsync();
                var contactDtos = new List<ClientDto>();

                foreach (var contact in contacts)
                    contactDtos.Add(new ClientDto
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Email = contact.Email,
                        Document = contact.Document,
                        Birth = contact.Birth
                    });

                return contactDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta de todos os clientes. Erro: {e.Message}");
                return null;
            }
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            try
            {
                var contact = await _contactService.GetByIdAsync(id);
                if (contact == null)
                    return null;

                return new ClientDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Email = contact.Email,
                    Document = contact.Document,
                    Birth = contact.Birth
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta dos clientes por Id. Erro: {e.Message}");
                return null;
            }
        }

        public async Task<ClientDto?> GetClientByEmailAsync(string email)
        {
            try
            {
                var contact = await _contactService.GetEmailCodeAsync(email);
                if (contact == null)
                    return null;

                return new ClientDto
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Email = contact.Email,
                    Document = contact.Document,
                    Birth = contact.Birth
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta dos clientes por Email. Erro: {e.Message}");
                return null;
            }
        }        
    }
}
