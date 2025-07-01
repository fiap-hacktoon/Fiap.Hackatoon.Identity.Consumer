using FIAP.TechChallenge.UserHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChallenge.UserHub.Infrastructure.Data
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql("DefaultConnection",
                                        new MySqlServerVersion(new Version(8, 0, 21)),
                                        mySqlOptions => mySqlOptions.MigrationsAssembly("FIAP.TechChallenge.UserHub.Infrastructure"));
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");

                // Configuração da chave primária
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Configuração das propriedades
                entity.Property(e => e.TypeRole)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Document)
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                //entity.Property(e => e.Birth)
                //    .IsRequired();

                entity.Property(e => e.Creation)
                    .IsRequired();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");

                // Configuração da chave primária
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Configuração das propriedades
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TypeRole)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(50);
            });
        }
    }
}