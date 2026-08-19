using InfoMasterKonsole.Data;
using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoMasterKonsole.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private static CustomerRepository instance;
    private CustomerDbContext context;

    private CustomerRepository()
    {
        context = new CustomerDbContext();
        context.Database.EnsureCreated();
    }

    public static CustomerRepository GetInstance()
    {
        if (instance == null)
        {
            instance = new CustomerRepository();
        }

        return instance;
    }

    public void Add(Customer customer)
    {
        context.Customers.Add(customer);
        context.SaveChanges();
    }

    public List<Customer> GetAll()
    {
        return context.Customers
            .AsNoTracking()
            .ToList();
    }

    public Customer GetById(int id)
    {
        return context.Customers.Find(id);
    }

    public bool Exists(int id)
    {
        return context.Customers
            .Any(c => c.CustomerId == id);
    }

    public void Update(Customer customer)
    {
        Customer existing =
            context.Customers.Find(
                customer.CustomerId);

        if (existing == null)
        {
            return;
        }

        existing.CustomerName =
            customer.CustomerName;

        existing.Email =
            customer.Email;

        existing.PhoneNumber =
            customer.PhoneNumber;

        existing.Address =
            customer.Address;

        existing.CustomerType =
            customer.CustomerType;

        existing.RegistrationDate =
            customer.RegistrationDate;

        context.SaveChanges();
    }

    public void Delete(int id)
    {
        Customer customer =
            context.Customers.Find(id);

        if (customer != null)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
        }
    }
}