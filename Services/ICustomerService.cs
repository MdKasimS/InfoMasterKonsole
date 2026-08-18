using System;
using System.Collections.Generic;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// Service interface for customer business operations.
    /// </summary>
    public interface ICustomerService
    {
        ServiceResult Add(Customer customer);
        Customer? GetById(string id);
        IEnumerable<Customer> GetAll();
        ServiceResult Update(Customer customer);
        ServiceResult Delete(string id);
        IEnumerable<Customer> Search(Func<Customer, bool> predicate);
    }
}
