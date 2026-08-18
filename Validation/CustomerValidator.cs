using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Validation;

public class CustomerValidator
{
    private ICustomerRepository repository;

    public CustomerValidator(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    public bool Validate(
        Customer customer,
        bool isUpdate,
        out List<string> errors)
    {
        errors = new List<string>();

        if (customer.CustomerId <= 0)
        {
            errors.Add(
                "Customer ID must be greater than zero.");
        }
        else if (!isUpdate &&
                 repository.Exists(customer.CustomerId))
        {
            errors.Add(
                "Customer ID already exists.");
        }

        if (string.IsNullOrWhiteSpace(
            customer.CustomerName))
        {
            errors.Add(
                "Customer name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(
            customer.EmailAddress))
        {
            errors.Add(
                "Email address cannot be empty.");
        }

        if (customer.RegistrationDate >
            DateTime.Today)
        {
            errors.Add(
                "Registration date cannot be in the future.");
        }

        return errors.Count == 0;
    }
    public bool ValidateImportedCustomer(
    Customer customer,
    out List<string> errors)
    {
        return Validate(
            customer,
            false,
            out errors);
    }
}