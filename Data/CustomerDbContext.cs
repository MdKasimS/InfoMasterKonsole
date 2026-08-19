using Microsoft.EntityFrameworkCore;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Data;

public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        string projectDirectory =
            Directory.GetParent(
                AppContext.BaseDirectory)!
            .Parent!.Parent!.Parent!.FullName;

        string databasePath =
            Path.Combine(
                projectDirectory,
                "customers.db");

        optionsBuilder.UseSqlite(
            "Data Source=" + databasePath);
    }
}