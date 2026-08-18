using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Views;

public class SummaryView
{
    private ICustomerService service;

    public SummaryView(ICustomerService service)
    {
        this.service = service;
    }

    public void ShowSummary()
    {
        Console.Clear();

        Console.WriteLine("========================================");
        Console.WriteLine("          CUSTOMER SUMMARY");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            List<Customer> customers =
                service.GetAllCustomers();

            int totalCustomers = customers.Count;
            int regularCustomers = 0;
            int premiumCustomers = 0;
            int otherCustomers = 0;

            foreach (Customer customer in customers)
            {
                if (customer.CustomerType
                    .Equals(
                        "Regular",
                        StringComparison.OrdinalIgnoreCase))
                {
                    regularCustomers++;
                }
                else if (customer.CustomerType
                    .Equals(
                        "Premium",
                        StringComparison.OrdinalIgnoreCase))
                {
                    premiumCustomers++;
                }
                else
                {
                    otherCustomers++;
                }
            }

            Console.WriteLine(
                "Total Customers   : " + totalCustomers);

            Console.WriteLine(
                "Regular Customers : " + regularCustomers);

            Console.WriteLine(
                "Premium Customers : " + premiumCustomers);

            Console.WriteLine(
                "Other Types       : " + otherCustomers);
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Unable to generate customer summary.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}