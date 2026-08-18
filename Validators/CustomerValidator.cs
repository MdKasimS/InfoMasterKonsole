using System;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Validators
{
    /// <summary>
    /// Concrete validator for Customer entities.
    /// Performs simple, synchronous validation checks and returns a ValidationResult.
    /// Does not access databases or perform uniqueness checks.
    /// </summary>
    public class CustomerValidator : ICustomerValidator
    {
        public ValidationResult Validate(Customer customer)
        {
            var result = new ValidationResult();
            if (customer == null)
            {
                result.IsValid = false;
                result.Errors.Add("Customer is null.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(customer.Id))
            {
                result.IsValid = false;
                result.Errors.Add("Customer ID must be provided.");
            }

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                result.IsValid = false;
                result.Errors.Add("Customer name must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                result.IsValid = false;
                result.Errors.Add("Email address must not be empty.");
            }

            // Registration date must not be in the future.
            // Compare only Date portion to allow same-day registrations across timezones.
            var today = DateTime.Today;
            if (customer.RegistrationDate.Date > today)
            {
                result.IsValid = false;
                result.Errors.Add("Registration date cannot be in the future.");
            }

            return result;
        }
    }
}
