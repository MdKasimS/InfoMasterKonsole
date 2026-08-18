using System;
using System.Collections.Generic;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Repositories
{
    /// <summary>
    /// Repository interface for customer persistence operations. No implementation in Phase 1.
    /// </summary>
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer? GetById(string id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(string id);
        IEnumerable<Customer> Search(Func<Customer, bool> predicate);
    }
}
