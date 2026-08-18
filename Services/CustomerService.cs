using System;
using System.Collections.Generic;
using System.Linq;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Repositories;
using InfoMasterKonsole.Validators;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// Simple synchronous customer service implementing business rules using repository and validator.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly ICustomerValidator _validator;

        public CustomerService(ICustomerRepository repository, ICustomerValidator validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public ServiceResult Add(Customer customer)
        {
            if (customer == null) return ServiceResult.Failure("Customer is null.");

            // Validate basic fields
            var validation = _validator.Validate(customer);
            if (!validation.IsValid)
            {
                return ServiceResult.Failure(validation.Errors.ToArray());
            }

            // Check uniqueness of ID
            if (_repository.Exists(customer.Id))
            {
                return ServiceResult.Failure("Customer ID already exists.");
            }

            try
            {
                _repository.Add(customer);
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Failed to add customer: " + ex.Message);
            }
        }

        public Customer? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return _repository.GetById(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _repository.GetAll();
        }

        public ServiceResult Update(Customer customer)
        {
            if (customer == null) return ServiceResult.Failure("Customer is null.");

            if (!_repository.Exists(customer.Id))
            {
                return ServiceResult.Failure("Customer does not exist.");
            }

            var validation = _validator.Validate(customer);
            if (!validation.IsValid)
            {
                return ServiceResult.Failure(validation.Errors.ToArray());
            }

            try
            {
                _repository.Update(customer);
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Failed to update customer: " + ex.Message);
            }
        }

        public ServiceResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return ServiceResult.Failure("Customer ID is required.");

            if (!_repository.Exists(id))
            {
                // Handle gracefully
                return ServiceResult.Failure("Customer not found.");
            }

            try
            {
                _repository.Delete(id);
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Failed to delete customer: " + ex.Message);
            }
        }

        public IEnumerable<Customer> Search(Func<Customer, bool> predicate)
        {
            if (predicate == null) return Enumerable.Empty<Customer>();
            return _repository.Search(predicate);
        }
    }
}
