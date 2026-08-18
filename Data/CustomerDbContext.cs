using Microsoft.EntityFrameworkCore;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Data;

public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=customers.db");
    }
}