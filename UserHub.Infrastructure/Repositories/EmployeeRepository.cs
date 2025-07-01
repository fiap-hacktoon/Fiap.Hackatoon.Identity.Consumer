using FIAP.TechChallenge.UserHub.Domain.Entities;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.UserHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChallenge.UserHub.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ContactsDbContext _context;

        public EmployeeRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee contact)
        {
            await _context.Employees.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee contact)
        {
            _context.Employees.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public async Task<IEnumerable<Employee>> GetAllAsync()
            => await _context.Employees.ToListAsync();

        public async Task<IEnumerable<Employee>> GetByEmailAsync(string email)
            => await _context.Employees.Where(c => c.Email == email).ToListAsync();

        public async Task DeleteAsync(int id)
        {
            var contact = await GetByIdAsync(id);
            _context.Employees.Remove(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetByRoleAsync(TypeRole role)
            => await _context.Employees.Where(c => c.TypeRole == (int)role).ToListAsync();
    }
}