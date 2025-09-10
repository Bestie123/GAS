using GAS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GAS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<AppRequest> Requests => Set<AppRequest>();
        public DbSet<Equipment> Equipment => Set<Equipment>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Description> Descriptions => Set<Description>();
        public DbSet<Work> Works => Set<Work>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var cfg = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                var cs = cfg.GetConnectionString("Default") ?? "Server=.;Database=GAS;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(cs);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Индексы и ограничения
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Связи Request -> Client (многие к одному)
            modelBuilder.Entity<AppRequest>()
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

