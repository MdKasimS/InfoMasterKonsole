using System;
using System.Collections.Generic;
using InfoMasterKonsole.Forms;
using InfoMasterKonsole.ViewModels;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Console view for searching customers.
    /// </summary>
    public class SearchView : IView
    {
        private readonly SearchViewModel _searchViewModel;
        private readonly SearchForm _searchForm;

        public SearchView(SearchViewModel searchViewModel)
        {
            _searchViewModel = searchViewModel ?? throw new ArgumentNullException(nameof(searchViewModel));
            _searchForm = new SearchForm();
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\nSearch Customers");
                Console.WriteLine("1) Search");
                Console.WriteLine("0) Back");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        ExecuteSearch();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ExecuteSearch()
        {
            try
            {
                var (criterion, value) = _searchForm.CollectSearchCriteria();
                var results = _searchViewModel.Search(criterion, value);
                PrintCustomers(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing search: {ex.Message}");
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
                Console.WriteLine("No results found.");
                return;
            }

            Console.WriteLine("\nSearch results:");
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
