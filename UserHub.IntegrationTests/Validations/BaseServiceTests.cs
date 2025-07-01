using FIAP.TechChallenge.UserHub.Infrastructure.Data;
using FIAP.TechChallenge.UserHub.IntegrationTest.Config;

namespace FIAP.TechChallenge.UserHub.IntegrationTest.Validations
{
    public abstract class BaseServiceTests : IDisposable
    {
        protected ContactsDbContext _context;
        private readonly TestDbMemoryFixture _dbFixture;

        public BaseServiceTests()
        {
            _dbFixture = new TestDbMemoryFixture();
            _context = _dbFixture.ContactsDbContext;
        }

        protected async Task SaveChanges()
            => await _context.SaveChangesAsync();

        public virtual void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
