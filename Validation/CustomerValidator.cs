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
    // Validates a customer's field values only (name, email, ID format,
    // registration date). Deliberately skips the "ID already exists"
    // check, because on import a duplicate ID should be treated as a
    // skip, not a validation failure. Duplicate detection is handled
    // separately by the caller.
    public bool ValidateImportedCustomer(
        Customer customer,
        out List<string> errors)
    {
        errors = new List<string>();

        if (customer.CustomerId <= 0)
        {
            errors.Add(
                "Customer ID must be greater than zero.");
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
}