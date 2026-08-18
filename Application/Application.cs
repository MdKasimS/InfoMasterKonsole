using System;
using InfoMasterKonsole.Views;
using InfoMasterKonsole.ViewModels;
using InfoMasterKonsole.Services;
using InfoMasterKonsole.Repositories;
using InfoMasterKonsole.Validators;
using InfoMasterKonsole.Data;

namespace InfoMasterKonsole.Application
{
    /// <summary>
    /// Application coordinator implementing Singleton pattern.
    /// Responsible for wiring objects and basic navigation. No business logic here.
    /// </summary>
    public class Application
    {
        private static readonly object _lock = new object();
        private static Application? _instance;

        public static Application Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Application();
                        }
                    }
                }
                return _instance!;
            }
        }

        // Application components
        private readonly ICustomerRepository _repository;
        private readonly ICustomerValidator _validator;
        private readonly ICustomerService _service;

        private readonly JsonCustomerSerializer _jsonSerializer;
        private readonly XmlCustomerSerializer _xmlSerializer;

        private readonly CustomerViewModel _customerViewModel;
        private readonly SearchViewModel _searchViewModel;
        private readonly ExportViewModel _exportViewModel; // created when needed
        private readonly ImportViewModel _importJsonViewModel; // created when needed
        private readonly SummaryViewModel _summaryViewModel;

        // Views
        private readonly CustomerView _customerView;
        private readonly SearchView _searchView;
        private readonly ExportView _exportView;
        private readonly ImportView _importView;
        private readonly SummaryView _summaryView;

        private Application()
        {
            // Initialize database (create file and schema if needed)
            try
            {
                var initializer = new DbInitializer();
                initializer.Initialize();
            }
            catch
            {
                // Swallow initialization errors here; views/services will report problems if operations fail.
            }

            // Compose concrete instances (manual wiring, no DI container)
            _repository = new CustomerRepository();
            _validator = new CustomerValidator();
            _service = new CustomerService(_repository, _validator);

            _jsonSerializer = new JsonCustomerSerializer();
            _xmlSerializer = new XmlCustomerSerializer();

            _customerViewModel = new CustomerViewModel(_service);
            _searchViewModel = new SearchViewModel(_service);
            _summaryViewModel = new SummaryViewModel(_service);

            _customerView = new CustomerView(_customerViewModel, _searchViewModel);
            _searchView = new SearchView(_searchViewModel);
            _exportView = new ExportView(_service, _jsonSerializer, _xmlSerializer);
            _importView = new ImportView(_service, _jsonSerializer, _xmlSerializer);
            _summaryView = new SummaryView(_summaryViewModel);
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nInfoMasterKonsole - Customer Data Exchange System");
                Console.WriteLine("1) Customer Management");
                Console.WriteLine("2) Search");
                Console.WriteLine("3) Import");
                Console.WriteLine("4) Export");
                Console.WriteLine("5) Summary / Statistics");
                Console.WriteLine("0) Exit");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            _customerView.Show();
                            break;
                        case "2":
                            _searchView.Show();
                            break;
                        case "3":
                            _importView.Show();
                            break;
                        case "4":
                            _exportView.Show();
                            break;
                        case "5":
                            _summaryView.Show();
                            break;
                        case "0":
                            Console.WriteLine("Exiting application.");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }
    }
}
