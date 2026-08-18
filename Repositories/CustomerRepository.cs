using System;
using System.Collections.Generic;
using System.Linq;
using InfoMasterKonsole.Data;
using InfoMasterKonsole.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoMasterKonsole.Repositories
{
    /// <summary>
    /// EF Core implementation of ICustomerRepository.
    /// Simple synchronous methods and minimal logic as required by Phase: repository layer only.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext? _externalContext;

        public CustomerRepository()
        {
            _externalContext = null;
        }

        // Optional constructor for tests or external context injection (not a DI container)
        public CustomerRepository(CustomerDbContext context)
        {
            _externalContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        private CustomerDbContext CreateContext()
        {
            return _externalContext ?? new CustomerDbContext();
        }

        public IEnumerable<Customer> GetAll()
        {
            using (var ctx = CreateContext())
            {
                return ctx.Customers.AsNoTracking().ToList();
            }
        }

        public Customer? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            using (var ctx = CreateContext())
            {
                return ctx.Customers.AsNoTracking().FirstOrDefault(c => c.Id == id);
            }
        }

        public void Add(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            using (var ctx = CreateContext())
            {
                ctx.Customers.Add(customer);
                ctx.SaveChanges();
            }
        }

        public void Update(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            using (var ctx = CreateContext())
            {
                ctx.Customers.Update(customer);
                ctx.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            using (var ctx = CreateContext())
            {
                var entity = ctx.Customers.FirstOrDefault(c => c.Id == id);
                if (entity != null)
                {
                    ctx.Customers.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }

        public IEnumerable<Customer> Search(Func<Customer, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            using (var ctx = CreateContext())
            {
                // Load into memory then apply predicate to allow arbitrary predicates
                return ctx.Customers.AsNoTracking().ToList().Where(predicate).ToList();
            }
        }

        public bool Exists(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            using (var ctx = CreateContext())
            {
                return ctx.Customers.AsNoTracking().Any(c => c.Id == id);
            }
        }
    }
}
