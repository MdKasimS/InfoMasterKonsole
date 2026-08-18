using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Views;

public class SearchView
{
    private ICustomerService service;

    public SearchView(ICustomerService service)
    {
        this.service = service;
    }

    public void Search()
    {
        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("          SEARCH CUSTOMERS");
        Console.WriteLine("========================================");
        Console.WriteLine();

        Console.WriteLine("Enter a name, email, phone number,");
        Console.WriteLine("or customer type.");
        Console.WriteLine();

        Console.Write("Search: ");

        string searchText =
            Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            Console.WriteLine();
            Console.WriteLine(
                "Search value cannot be empty.");

            Console.WriteLine();
            Console.WriteLine(
                "Press Enter to continue.");

            Console.ReadLine();
            return;
        }

        try
        {
            List<Customer> customers =
                service.SearchCustomers(searchText);

            Console.WriteLine();

            if (customers.Count == 0)
            {
                Console.WriteLine(
                    "No matching customers found.");
            }
            else
            {
                Console.WriteLine(
                    "Search Results:");

                foreach (Customer customer in customers)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "----------------------------------------");

                    Console.WriteLine(
                        "ID       : " + customer.CustomerId);

                    Console.WriteLine(
                        "Name     : " + customer.CustomerName);

                    Console.WriteLine(
                        "Email    : " + customer.EmailAddress);

                    Console.WriteLine(
                        "Phone    : " + customer.PhoneNumber);

                    Console.WriteLine(
                        "Type     : " + customer.CustomerType);

                    Console.WriteLine(
                        "Address  : " + customer.Address);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Unable to search customer records.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}