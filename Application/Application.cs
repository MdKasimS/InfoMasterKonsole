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
        private readonly Views.MainMenuView _mainMenuView;

        private Application()
        {
            // Initialize database (create file and schema if needed)
            try
            {
                var initializer = new DbInitializer();
                var initResult = initializer.Initialize();
                if (!initResult.IsSuccess)
                {
                    // Log and inform user with a friendly message
                    Console.WriteLine("Warning: database initialization reported issues. Some operations may fail.");
                    foreach (var e in initResult.Errors)
                    {
                        Console.WriteLine(" - " + e);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log unexpected initialization exception and continue; views will handle operational errors.
                Exceptions.ExceptionLogger.Log(ex);
                Console.WriteLine("Warning: unexpected error during database initialization. See logs for details.");
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

            _mainMenuView = new Views.MainMenuView(_customerView, _importView, _exportView, _searchView, _summaryView);
        }

        public void Run()
        {
            _mainMenuView.Show();
        }
    }
}
