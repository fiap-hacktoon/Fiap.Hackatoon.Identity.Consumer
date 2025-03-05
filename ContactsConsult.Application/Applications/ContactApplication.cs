using FIAP.TechChallenge.ContactsConsult.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Applications;
using FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ContactsConsult.Application.Applications
{
    public class ContactApplication(IContactService contactService, ILogger<ContactApplication> logger) : IContactApplication
    {
        private readonly IContactService _contactService = contactService;

        private readonly ILogger<ContactApplication> _logger = logger;

        public async Task<IEnumerable<ContactDto>> GetAllContactsAsync()
        {
            try
            {
                var contacts = await _contactService.GetAllAsync();
                var contactDtos = new List<ContactDto>();

                foreach (var contact in contacts)
                    contactDtos.Add(new ContactDto
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Email = contact.Email,
                        AreaCode = contact.AreaCode,
                        Phone = contact.Phone
                    });

                return contactDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Ocorreu um erro na consulta de todos os contatos. Erro: {e.Message}");
                return null;
            }
        }

        public async Task<ContactDto?> GetContactByIdAsync(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            if (contact == null)
                return null;

            return new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                AreaCode = contact.AreaCode,
                Phone = contact.Phone
            };
        }

        public async Task<IEnumerable<ContactDto>> GetContactsByAreaCodeAsync(string areaCode)
        {
            var contacts = await _contactService.GetByAreaCodeAsync(areaCode);
            var contactDtos = contacts.Select(contact => new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                AreaCode = contact.AreaCode,
                Phone = contact.Phone
            });

            return contactDtos;
        }
    }
}