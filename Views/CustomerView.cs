using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.ViewModels;

namespace InfoMasterKonsole.Views;

public class CustomerView
{
    private ICustomerService service;

    public CustomerView(ICustomerService service)
    {
        this.service = service;
    }
    public CustomerViewModel GetCustomerFromInput()
    {
        CustomerViewModel viewModel =
            new CustomerViewModel();

        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("       ADD CUSTOMER");
        Console.WriteLine("==============================");

        viewModel.Customer.CustomerId =
            ReadInteger("Customer ID: ");

        Console.Write("Customer Name: ");
        viewModel.Customer.CustomerName =
            Console.ReadLine() ?? "";

        Console.Write("Email Address: ");
        viewModel.Customer.EmailAddress =
            Console.ReadLine() ?? "";

        Console.Write("Phone Number: ");
        viewModel.Customer.PhoneNumber =
            Console.ReadLine() ?? "";

        Console.Write("Address: ");
        viewModel.Customer.Address =
            Console.ReadLine() ?? "";

        Console.Write("Customer Type: ");
        viewModel.Customer.CustomerType =
            Console.ReadLine() ?? "";

        viewModel.Customer.RegistrationDate =
            ReadDate("Registration Date (yyyy-MM-dd): ");

        return viewModel;
    }

    private int ReadInteger(string message)
    {
        int value;

        while (true)
        {
            Console.Write(message);

            if (int.TryParse(
                Console.ReadLine(),
                out value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid number.");
        }
    }

    private DateTime ReadDate(string message)
    {
        DateTime value;

        while (true)
        {
            Console.Write(message);

            if (DateTime.TryParse(
                Console.ReadLine(),
                out value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid date.");
        }
    }

    public void AddCustomer()
    {
        CustomerViewModel viewModel =
            GetCustomerFromInput();

        List<string> errors;

        try
        {
            bool success =
                service.AddCustomer(
                    viewModel.Customer,
                    out errors);

            if (success)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Customer added successfully.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Customer could not be added.");

                foreach (string error in errors)
                {
                    Console.WriteLine("- " + error);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "An error occurred while saving the customer.");

            Console.WriteLine(
                exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
    public void ViewAllCustomers()
    {
        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           CUSTOMER DETAILS");
        Console.WriteLine("========================================");

        try
        {
            List<Customer> customers =
                service.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (Customer customer in customers)
                {
                    DisplayCustomer(customer);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Unable to retrieve customer data.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
    private void DisplayCustomer(Customer customer)
    {
        Console.WriteLine();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Customer ID       : " + customer.CustomerId);
        Console.WriteLine("Customer Name     : " + customer.CustomerName);
        Console.WriteLine("Email Address     : " + customer.EmailAddress);
        Console.WriteLine("Phone Number      : " + customer.PhoneNumber);
        Console.WriteLine("Address           : " + customer.Address);
        Console.WriteLine("Customer Type     : " + customer.CustomerType);
        Console.WriteLine("Registration Date : " +
            customer.RegistrationDate.ToString("yyyy-MM-dd"));
        Console.WriteLine("----------------------------------------");
    }
    public void UpdateCustomer()
    {
        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           UPDATE CUSTOMER");
        Console.WriteLine("========================================");

        int customerId =
            ReadInteger("Enter Customer ID: ");

        try
        {
            Customer? customer =
                service.GetCustomerById(customerId);

            if (customer == null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Customer not found.");

                Console.WriteLine();
                Console.WriteLine(
                    "Press Enter to continue.");

                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Leave a field empty to keep its current value.");

            Console.WriteLine();

            Console.Write(
                "Customer Name [" +
                customer.CustomerName +
                "]: ");

            string name =
                Console.ReadLine() ?? "";

            if (name != "")
            {
                customer.CustomerName = name;
            }

            Console.Write(
                "Email Address [" +
                customer.EmailAddress +
                "]: ");

            string email =
                Console.ReadLine() ?? "";

            if (email != "")
            {
                customer.EmailAddress = email;
            }

            Console.Write(
                "Phone Number [" +
                customer.PhoneNumber +
                "]: ");

            string phone =
                Console.ReadLine() ?? "";

            if (phone != "")
            {
                customer.PhoneNumber = phone;
            }

            Console.Write(
                "Address [" +
                customer.Address +
                "]: ");

            string address =
                Console.ReadLine() ?? "";

            if (address != "")
            {
                customer.Address = address;
            }

            Console.Write(
                "Customer Type [" +
                customer.CustomerType +
                "]: ");

            string customerType =
                Console.ReadLine() ?? "";

            if (customerType != "")
            {
                customer.CustomerType = customerType;
            }

            Console.Write(
                "Registration Date [" +
                customer.RegistrationDate.ToString("yyyy-MM-dd") +
                "]: ");

            string dateInput =
                Console.ReadLine() ?? "";

            if (dateInput != "")
            {
                DateTime registrationDate;

                if (DateTime.TryParse(
                    dateInput,
                    out registrationDate))
                {
                    customer.RegistrationDate =
                        registrationDate;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "Invalid date. Customer was not updated.");

                    Console.ReadLine();
                    return;
                }
            }

            List<string> errors;

            bool success =
                service.UpdateCustomer(
                    customer,
                    out errors);

            Console.WriteLine();

            if (success)
            {
                Console.WriteLine(
                    "Customer updated successfully.");
            }
            else
            {
                Console.WriteLine(
                    "Customer could not be updated.");

                foreach (string error in errors)
                {
                    Console.WriteLine("- " + error);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "An error occurred while updating the customer.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
    public void DeleteCustomer()
    {
        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           DELETE CUSTOMER");
        Console.WriteLine("========================================");

        int customerId =
            ReadInteger("Enter Customer ID: ");

        try
        {
            Customer? customer =
                service.GetCustomerById(customerId);

            if (customer == null)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Customer not found.");
            }
            else
            {
                Console.WriteLine();
                DisplayCustomer(customer);

                Console.WriteLine();
                Console.Write(
                    "Are you sure you want to delete this customer? (Y/N): ");

                string confirmation =
                    Console.ReadLine() ?? "";

                if (confirmation.ToUpper() == "Y")
                {
                    bool success =
                        service.DeleteCustomer(customerId);

                    Console.WriteLine();

                    if (success)
                    {
                        Console.WriteLine(
                            "Customer deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Customer could not be deleted.");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "Delete operation cancelled.");
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "An error occurred while deleting the customer.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}