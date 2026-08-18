using InfoMasterKonsole.Data;
using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Repositories;

public class CustomerRepository : ICustomerRepository
{
    public void Add(Customer customer)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }
    }

    public List<Customer> GetAll()
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            return context.Customers.ToList();
        }
    }

    public Customer GetById(int customerId)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            return context.Customers.Find(customerId);
        }
    }

    public List<Customer> Search(string searchText)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            return context.Customers
                .Where(c =>
                    c.CustomerName.Contains(searchText) ||
                    c.EmailAddress.Contains(searchText) ||
                    c.PhoneNumber.Contains(searchText) ||
                    c.CustomerType.Contains(searchText))
                .ToList();
        }
    }

    public void Update(Customer customer)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            context.Customers.Update(customer);
            context.SaveChanges();
        }
    }

    public void Delete(int customerId)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            Customer customer = context.Customers.Find(customerId);

            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }
    }

    public bool Exists(int customerId)
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            return context.Customers.Any(c => c.CustomerId == customerId);
        }
    }

    public int Count()
    {
        using (CustomerDbContext context = new CustomerDbContext())
        {
            return context.Customers.Count();
        }
    }
}