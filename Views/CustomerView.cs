using System;
using System.Collections.Generic;
using InfoMasterKonsole.Forms;
using InfoMasterKonsole.ViewModels;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Views;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Console view for customer operations: add, view all, update, delete.
    /// Uses Forms for input and ViewModels for coordination.
    /// </summary>
    public class CustomerView : IView
    {
        private readonly CustomerViewModel _customerViewModel;
        private readonly SearchViewModel _searchViewModel;
        private readonly CustomerForm _customerForm;
        private readonly UpdateCustomerForm _updateForm;
        private readonly SearchForm _searchForm;

        public CustomerView(CustomerViewModel customerViewModel, SearchViewModel searchViewModel)
        {
            _customerViewModel = customerViewModel ?? throw new ArgumentNullException(nameof(customerViewModel));
            _searchViewModel = searchViewModel ?? throw new ArgumentNullException(nameof(searchViewModel));
            _customerForm = new CustomerForm();
            _updateForm = new UpdateCustomerForm();
            _searchForm = new SearchForm();
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\nCustomer Management");
                Console.WriteLine("1) Add Customer");
                Console.WriteLine("2) View All Customers");
                Console.WriteLine("3) Update Customer");
                Console.WriteLine("4) Delete Customer");
                Console.WriteLine("5) Search Customers");
                Console.WriteLine("0) Back");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddCustomerFlow();
                            break;
                        case "2":
                            ViewAllFlow();
                            break;
                        case "3":
                            UpdateCustomerFlow();
                            break;
                        case "4":
                            DeleteCustomerFlow();
                            break;
                        case "5":
                            SearchCustomersFlow();
                            break;
                        case "0":
                            return; // exit the view
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                    Console.WriteLine("An unexpected error occurred. See logs for details.");
                }
            }
        }

        private void AddCustomerFlow()
        {
            try
            {
                var customer = _customerForm.CreateCustomerFromConsole();
                var result = _customerViewModel.AddCustomer(customer);
                if (result.IsSuccess)
                {
                    Console.WriteLine("Customer added successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to add customer:");
                    foreach (var err in result.Errors)
                    {
                        Console.WriteLine(" - " + err);
                    }
                }
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Error adding customer. See logs for details.");
            }
        }

        private void ViewAllFlow()
        {
            try
            {
                var customers = _customerViewModel.GetAllCustomers();
                PrintCustomers(customers);
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Error retrieving customers. See logs for details.");
            }
        }

        private void UpdateCustomerFlow()
        {
            try
            {
                Console.Write("Enter Customer ID to update: ");
                var id = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine("Customer ID is required.");
                    return;
                }

                var existing = _customerViewModel.GetCustomerById(id);
                if (existing == null)
                {
                    Console.WriteLine("Customer not found.");
                    return;
                }

                var updated = _updateForm.UpdateCustomerFromConsole(existing);
                var result = _customerViewModel.UpdateCustomer(updated);
                if (result.IsSuccess)
                {
                    Console.WriteLine("Customer updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update customer:");
                    foreach (var err in result.Errors)
                    {
                        Console.WriteLine(" - " + err);
                    }
                }
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Error updating customer. See logs for details.");
            }
        }

        private void DeleteCustomerFlow()
        {
            try
            {
                Console.Write("Enter Customer ID to delete: ");
                var id = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine("Customer ID is required.");
                    return;
                }

                var existing = _customerViewModel.GetCustomerById(id);
                if (existing == null)
                {
                    Console.WriteLine("Customer not found.");
                    return;
                }

                Console.Write($"Are you sure you want to delete customer '{existing.Name}' (ID: {existing.Id})? (y/N): ");
                var confirm = Console.ReadLine();
                if (string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase) || string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    var result = _customerViewModel.DeleteCustomer(id);
                    if (result.IsSuccess)
                    {
                        Console.WriteLine("Customer deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete customer:");
                        foreach (var err in result.Errors)
                        {
                            Console.WriteLine(" - " + err);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Delete cancelled.");
                }
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Error deleting customer. See logs for details.");
            }
        }

        private void SearchCustomersFlow()
        {
            try
            {
                var (criterion, value) = _searchForm.CollectSearchCriteria();
                var results = _searchViewModel.Search(criterion, value);
                PrintCustomers(results);
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Error searching customers. See logs for details.");
            }
        }

        private void PrintCustomers(IEnumerable<Customer> customers)
        {
            if (customers == null)
            {
                Console.WriteLine("No customers to display.");
                return;
            }

            var list = new List<Customer>(customers);
            if (list.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            Console.WriteLine("\nCustomers:");
            foreach (var c in list)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine($"ID: {c.Id}");
                Console.WriteLine($"Name: {c.Name}");
                Console.WriteLine($"Email: {c.Email}");
                Console.WriteLine($"Phone: {c.Phone}");
                Console.WriteLine($"Address: {c.Address}");
                Console.WriteLine($"Customer Type: {c.CustomerType}");
                Console.WriteLine($"Registration Date: {c.RegistrationDate:yyyy-MM-dd}");
            }
            Console.WriteLine("------------------------------");
        }
    }
}
