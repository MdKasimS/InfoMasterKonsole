using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Validation;

public class CustomerValidator : ICustomerValidator
{
    public List<string> Validate(
        Customer customer,
        ICustomerRepository repository,
        bool checkId)
    {
        List<string> errors =
            new List<string>();

        // Customer ID
        if (checkId)
        {
            if (customer.CustomerId <= 0)
            {
                errors.Add(
                    "Customer ID must be greater than zero.");
            }
            else if (repository.Exists(
                customer.CustomerId))
            {
                errors.Add(
                    "Customer ID must be unique.");
            }
        }

        // Customer Name
        if (string.IsNullOrWhiteSpace(
            customer.CustomerName))
        {
            errors.Add(
                "Customer name cannot be empty.");
        }
        else
        {
            foreach (char character
                     in customer.CustomerName)
            {
                if (!char.IsLetter(character) &&
                    character != ' ')
                {
                    errors.Add(
                        "Customer name should contain only letters.");
                    break;
                }
            }
        }

        // Email
        if (string.IsNullOrWhiteSpace(
            customer.Email))
        {
            errors.Add(
                "Email address cannot be empty.");
        }
        else if (!customer.Email.Contains("@"))
        {
            errors.Add(
                "Email address must contain @.");
        }

        // Phone
        if (string.IsNullOrWhiteSpace(
            customer.PhoneNumber))
        {
            errors.Add(
                "Phone number cannot be empty.");
        }
        else
        {
            foreach (char character
                     in customer.PhoneNumber)
            {
                if (!char.IsDigit(character))
                {
                    errors.Add(
                        "Phone number should contain only numbers.");
                    break;
                }
            }
        }

        // Address
        if (string.IsNullOrWhiteSpace(
            customer.Address))
        {
            errors.Add(
                "Address cannot be empty.");
        }

        // Customer Type
        if (string.IsNullOrWhiteSpace(
            customer.CustomerType))
        {
            errors.Add(
                "Customer type cannot be empty.");
        }

        // Registration Date
        if (customer.RegistrationDate >
            DateTime.Now)
        {
            errors.Add(
                "Registration date cannot be a future date.");
        }

        return errors;
    }
}