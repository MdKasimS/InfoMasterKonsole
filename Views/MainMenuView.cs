using System;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Top-level console menu that delegates to child views.
    /// </summary>
    public class MainMenuView
    {
        private readonly CustomerView _customerView;
        private readonly ImportView _importView;
        private readonly ExportView _exportView;
        private readonly SearchView _searchView;
        private readonly SummaryView _summaryView;

        public MainMenuView(CustomerView customerView, ImportView importView, ExportView exportView, SearchView searchView, SummaryView summaryView)
        {
            _customerView = customerView ?? throw new ArgumentNullException(nameof(customerView));
            _importView = importView ?? throw new ArgumentNullException(nameof(importView));
            _exportView = exportView ?? throw new ArgumentNullException(nameof(exportView));
            _searchView = searchView ?? throw new ArgumentNullException(nameof(searchView));
            _summaryView = summaryView ?? throw new ArgumentNullException(nameof(summaryView));
        }

        public void Show()
        {
            while (true)
            {
                ScreenHelper.ShowTitle("CUSTOMER DATA EXCHANGE SYSTEM");
                Console.WriteLine("1) Customer Management");
                Console.WriteLine("2) Import Data");
                Console.WriteLine("3) Export Data");
                Console.WriteLine("4) Search Customers");
                Console.WriteLine("5) Customer Summary");
                Console.WriteLine("6) Exit");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        _customerView.Show();
                        break;
                    case "2":
                        Console.Clear();
                        _importView.Show();
                        break;
                    case "3":
                        Console.Clear();
                        _exportView.Show();
                        break;
                    case "4":
                        Console.Clear();
                        _searchView.Show();
                        break;
                    case "5":
                        Console.Clear();
                        _summaryView.Show();
                        break;
                    case "6":
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting application.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        ScreenHelper.PauseAndClear();
                        break;
                }
            }
        }
    }
}
