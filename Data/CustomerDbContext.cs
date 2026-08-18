using System;
using System.IO;
using InfoMasterKonsole.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoMasterKonsole.Data
{
    /// <summary>
    /// EF Core DbContext for Customer persistence using SQLite.
    /// OnConfiguring sets up a file-based SQLite database located under the application's data folder.
    /// No DI container is used; consumers can instantiate CustomerDbContext directly when needed.
    /// </summary>
    public class CustomerDbContext : DbContext
    {
        private string GetDefaultDatabasePath()
        {
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataDir = Path.Combine(baseDir, "data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            var dbPath = Path.Combine(dataDir, "customers.db");
            return dbPath;
        }

        public CustomerDbContext()
        {
        }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = GetDefaultDatabasePath();
                var connectionString = $"Data Source={dbPath}";
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Simple, explicit mapping for Customer model. Keep rules simple and explainable.
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Name).HasMaxLength(200);
                entity.Property(c => c.Phone).HasMaxLength(50);
                entity.Property(c => c.Address).HasMaxLength(500);
                entity.Property(c => c.CustomerType).HasMaxLength(100);
                entity.Property(c => c.RegistrationDate).IsRequired();

                // Optionally add an index on Email for search performance
                entity.HasIndex(c => c.Email);
            });
        }
    }
}
