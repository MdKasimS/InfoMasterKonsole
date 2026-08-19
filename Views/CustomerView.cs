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

    public void AddCustomer()
    {
        CustomerViewModel viewModel = new CustomerViewModel(service);

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

        try
        {
            viewModel.Add();

            Console.WriteLine();

            if (viewModel.IsSuccess)
            {
                Console.WriteLine(
                    "Customer added successfully.");
            }
            else
            {
                Console.WriteLine(
                    "Customer could not be added.");

                foreach (string error in viewModel.ErrorMessages)
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

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    public void ViewAllCustomers()
    {
        CustomerViewModel viewModel = new CustomerViewModel(service);

        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           CUSTOMER DETAILS");
        Console.WriteLine("========================================");

        try
        {
            viewModel.LoadAll();

            if (viewModel.Customers.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (Customer customer in viewModel.Customers)
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

    public void UpdateCustomer()
    {
        CustomerViewModel viewModel = new CustomerViewModel(service);

        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           UPDATE CUSTOMER");
        Console.WriteLine("========================================");

        int customerId =
            ReadInteger("Enter Customer ID: ");

        try
        {
            bool found = viewModel.Load(customerId);

            if (!found)
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
                viewModel.Customer.CustomerName +
                "]: ");

            string name =
                Console.ReadLine() ?? "";

            if (name != "")
            {
                viewModel.Customer.CustomerName = name;
            }

            Console.Write(
                "Email Address [" +
                viewModel.Customer.EmailAddress +
                "]: ");

            string email =
                Console.ReadLine() ?? "";

            if (email != "")
            {
                viewModel.Customer.EmailAddress = email;
            }

            Console.Write(
                "Phone Number [" +
                viewModel.Customer.PhoneNumber +
                "]: ");

            string phone =
                Console.ReadLine() ?? "";

            if (phone != "")
            {
                viewModel.Customer.PhoneNumber = phone;
            }

            Console.Write(
                "Address [" +
                viewModel.Customer.Address +
                "]: ");

            string address =
                Console.ReadLine() ?? "";

            if (address != "")
            {
                viewModel.Customer.Address = address;
            }

            Console.Write(
                "Customer Type [" +
                viewModel.Customer.CustomerType +
                "]: ");

            string customerType =
                Console.ReadLine() ?? "";

            if (customerType != "")
            {
                viewModel.Customer.CustomerType = customerType;
            }

            Console.Write(
                "Registration Date [" +
                viewModel.Customer.RegistrationDate.ToString("yyyy-MM-dd") +
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
                    viewModel.Customer.RegistrationDate =
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

            viewModel.Update();

            Console.WriteLine();

            if (viewModel.IsSuccess)
            {
                Console.WriteLine(
                    "Customer updated successfully.");
            }
            else
            {
                Console.WriteLine(
                    "Customer could not be updated.");

                foreach (string error in viewModel.ErrorMessages)
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
        CustomerViewModel viewModel = new CustomerViewModel(service);

        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("           DELETE CUSTOMER");
        Console.WriteLine("========================================");

        int customerId =
            ReadInteger("Enter Customer ID: ");

        try
        {
            bool found = viewModel.Load(customerId);

            if (!found)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Customer not found.");
            }
            else
            {
                Console.WriteLine();
                DisplayCustomer(viewModel.Customer);

                Console.WriteLine();
                Console.Write(
                    "Are you sure you want to delete this customer? (Y/N): ");

                string confirmation =
                    Console.ReadLine() ?? "";

                if (confirmation.ToUpper() == "Y")
                {
                    bool success =
                        viewModel.Delete(customerId);

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
}
