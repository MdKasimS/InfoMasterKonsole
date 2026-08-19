using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.ViewModels;

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

        SummaryViewModel viewModel = new SummaryViewModel(service);

        try
        {
            viewModel.Load();

            Console.WriteLine(
                "Total Customers   : " + viewModel.TotalCustomers);

            Console.WriteLine(
                "Regular Customers : " + viewModel.RegularCustomers);

            Console.WriteLine(
                "Premium Customers : " + viewModel.PremiumCustomers);

            Console.WriteLine(
                "Other Types       : " + viewModel.OtherCustomers);
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
