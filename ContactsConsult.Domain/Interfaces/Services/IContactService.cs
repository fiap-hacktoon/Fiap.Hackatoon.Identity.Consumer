using FIAP.TechChallenge.ContactsConsult.Domain.Entities;

namespace FIAP.TechChallenge.ContactsConsult.Domain.Interfaces.Services
{
    public interface IContactService
    {
        Task<Contact> GetByIdAsync(int id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task<IEnumerable<Contact>> GetByAreaCodeAsync(string areaCode);

        Task<Contact> GetEmailCodeAsync(string email);
    }
}