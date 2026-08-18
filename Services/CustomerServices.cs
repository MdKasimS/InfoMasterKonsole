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
        if (!validator.Validate(customer, out errors))
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

    public Customer GetCustomerById(int customerId)
    {
        return repository.GetById(customerId);
    }

    public List<Customer> SearchCustomers(string searchText)
    {
        return repository.Search(searchText);
    }

    public bool UpdateCustomer(Customer customer, out List<string> errors)
    {
        errors = new List<string>();

        if (!repository.Exists(customer.CustomerId))
        {
            errors.Add("Customer does not exist.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(customer.CustomerName))
        {
            errors.Add("Customer name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(customer.EmailAddress))
        {
            errors.Add("Email address cannot be empty.");
        }

        if (customer.RegistrationDate > DateTime.Today)
        {
            errors.Add("Registration date cannot be in the future.");
        }

        if (errors.Count > 0)
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