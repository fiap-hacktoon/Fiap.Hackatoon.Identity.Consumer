using FIAP.TechChallenge.ContactsConsult.Domain.DTOs.EntityDTOs;

namespace FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Applications
{
    public interface IContactApplication
    {
        Task<IEnumerable<ContactDto>> GetAllContactsAsync();

        Task<ContactDto?> GetContactByIdAsync(int id);

        Task<IEnumerable<ContactDto>> GetContactsByAreaCodeAsync(string areaCode);
    }
}