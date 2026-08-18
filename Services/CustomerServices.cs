using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Validation;

namespace InfoMasterKonsole.Services;

public class CustomerService : ICustomerService
{
    private ICustomerRepository repository;
    private CustomerValidator validator;

    public CustomerService(
        ICustomerRepository repository,
        CustomerValidator validator)
    {
        this.repository = repository;
        this.validator = validator;
    }

    public bool AddCustomer(Customer customer, out List<string> errors)
    {
        if (!validator.Validate(customer, false, out errors))
        {
            return false;
        }

        repository.Add(customer);
        return true;
    }

    public List<Customer> GetAllCustomers()
    {
        return repository.GetAll();
    }

    public Customer? GetCustomerById(int customerId)
    {
        return repository.GetById(customerId);
    }

    public List<Customer> SearchCustomers(string searchText)
    {
        return repository.Search(searchText);
    }

    public bool UpdateCustomer(Customer customer, out List<string> errors)
    {
        if (!repository.Exists(customer.CustomerId))
        {
            errors = new List<string>();
            errors.Add("Customer does not exist.");
            return false;
        }

        if (!validator.Validate(customer, true, out errors))
        {
            return false;
        }

        repository.Update(customer);
        return true;
    }

    public bool DeleteCustomer(int customerId)
    {
        if (!repository.Exists(customerId))
        {
            return false;
        }

        repository.Delete(customerId);
        return true;
    }

    public int GetCustomerCount()
    {
        return repository.Count();
    }
}