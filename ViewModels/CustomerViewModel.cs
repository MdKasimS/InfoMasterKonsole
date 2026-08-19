using System;
using System.Collections.Generic;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Coordinates customer operations between Views (UI) and ICustomerService.
    /// Does not perform validation, persistence or UI work directly.
    /// </summary>
    public class CustomerViewModel
    {
        private readonly ICustomerService _service;

        public CustomerViewModel(ICustomerService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public ServiceResult AddCustomer(Customer customer)
        {
            return _service.Add(customer);
        }

        public Customer? GetCustomerById(string id)
        {
            return _service.GetById(id);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _service.GetAll();
        }

        public ServiceResult UpdateCustomer(Customer customer)
        {
            return _service.Update(customer);
        }

        public ServiceResult DeleteCustomer(string id)
        {
            return _service.Delete(id);
        }
    }
}
