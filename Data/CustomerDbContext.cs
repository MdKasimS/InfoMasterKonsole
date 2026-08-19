using InfoMasterKonsole.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoMasterKonsole.Data;

public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        string path = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "customers.db"));

        optionsBuilder.UseSqlite(
            "Data Source=" + path);
    }
}