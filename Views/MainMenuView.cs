using InfoMasterKonsole;
using InfoMasterKonsole.Interfaces;

namespace InfoMasterKonsole.Views;

public class MainMenuView
{
    private ICustomerService service;

    public MainMenuView(ICustomerService service)
    {
        this.service = service;
    }

    public void Show()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("     CUSTOMER DATA EXCHANGE SYSTEM");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. View Customers");
            Console.WriteLine("3. Update Customer");
            Console.WriteLine("4. Delete Customer");
            Console.WriteLine("5. Search Customers");
            Console.WriteLine("6. Export Customer Data");
            Console.WriteLine("7. Import Customer Data");
            Console.WriteLine("8. Customer Summary");
            Console.WriteLine("9. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddCustomer();
                    break;

                case "2":
                    ViewCustomers();
                    break;

                case "3":
                    UpdateCustomer();
                    break;

                case "4":
                    DeleteCustomer();
                    break;

                case "5":
                    SearchCustomers();
                    break;

                case "6":
                    ExportData();
                    break;

                case "7":
                    ImportData();
                    break;

                case "8":
                    ShowSummary();
                    break;

                case "9":
                    running = false;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid menu choice.");
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void AddCustomer()
    {
        CustomerView customerView =
            new CustomerView(service);

        customerView.AddCustomer();
    }

    private void ViewCustomers()
    {
        CustomerView customerView =
            new CustomerView(service);

        customerView.ViewAllCustomers();
    }

    private void UpdateCustomer()
    {
        CustomerView customerView =
            new CustomerView(service);

        customerView.UpdateCustomer();
    }

    private void DeleteCustomer()
    {
        CustomerView customerView =
            new CustomerView(service);

        customerView.DeleteCustomer();
    }

    private void SearchCustomers()
    {
        SearchView searchView =
            new SearchView(service);

        searchView.Search();
    }

    private void ExportData()
    {
        Console.WriteLine();
        Console.WriteLine("Export Customer Data - coming next.");
        Console.ReadLine();
    }

    private void ImportData()
    {
        Console.WriteLine();
        Console.WriteLine("Import Customer Data - coming next.");
        Console.ReadLine();
    }

    private void ShowSummary()
    {
        Console.WriteLine();
        Console.WriteLine("Customer Summary - coming next.");
        Console.ReadLine();
    }
}