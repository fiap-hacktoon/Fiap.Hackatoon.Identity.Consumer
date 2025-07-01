using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChallenge.UserHub.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ContactsDbContext _context;

        public ClientRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Client contact)
        {
            await _context.Clients.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client contact)
        {
            _context.Clients.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
            => await _context.Clients.FindAsync(id);

        public async Task<IEnumerable<Client>> GetAllAsync()
            => await _context.Clients.ToListAsync();

        public async Task<IEnumerable<Client>> GetByEmailAsync(string email)
            => await _context.Clients.Where(c => c.Email == email).ToListAsync();

        public async Task DeleteAsync(int id)
        {
            var contact = await GetByIdAsync(id);
            _context.Clients.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}