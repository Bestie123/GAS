using System;
using System.IO;
using GAS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GAS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<AppRequest> Requests
                                    => Set<AppRequest>();
        public DbSet<Equipment> Equipment
                                    => Set<Equipment>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Description> Descriptions
                                    => Set<Description>();
        public DbSet<Work> Works => Set<Work>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var cs = cfg.GetConnectionString("Default")
                     ?? "Server=.;Database=GAS;Trusted_Connection=True;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(
                cs,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                )
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальный индекс по Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Явная настройка связи AppRequest → Client (многие-к-одному)
            modelBuilder.Entity<AppRequest>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Client)
                      .WithMany(c => c.Requests)
                      .HasForeignKey(r => r.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // При необходимости добавьте здесь конфигурацию для других сущностей
        }
    }
}

