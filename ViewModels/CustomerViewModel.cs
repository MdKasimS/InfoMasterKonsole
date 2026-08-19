using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Repositories;
using InfoMasterKonsole.Services;
using InfoMasterKonsole.Validation;
using InfoMasterKonsole.Views;

namespace InfoMasterKonsole.ViewModels;

public class CustomerViewModel
{
    private ICustomerRepository repository;
    private IFileService fileService;
    private ICustomerValidator validator;

    private MainMenuView mainMenu;
    private CustomerView customerView;
    private ImportExportView importExportView;

    private string jsonPath;
    private string xmlPath;

    public CustomerViewModel()
    {
        repository = CustomerRepository.GetInstance();
        fileService = new FileService();
        validator = new CustomerValidator();

        mainMenu = new MainMenuView();
        customerView = new CustomerView();
        importExportView = new ImportExportView();

        jsonPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "customers.json"));

        xmlPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "customers.xml"));
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            try
            {
                mainMenu.Show();

                string choice = mainMenu.ReadChoice();

                switch (choice)
                {
                    case "1":
                        CustomerManagement();
                        break;

                    case "2":
                        ImportExport();
                        break;

                    case "3":
                        Search();
                        break;

                    case "4":
                        Summary();
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("Application closed.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        customerView.Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                customerView.Pause();
            }
        }
    }

    private void CustomerManagement()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();

            Console.WriteLine("==========================================");
            Console.WriteLine("          CUSTOMER MANAGEMENT");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. View Customers");
            Console.WriteLine("3. Update Customer");
            Console.WriteLine("4. Delete Customer");
            Console.WriteLine("5. Back");
            Console.WriteLine("==========================================");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();

            try
            {
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
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        customerView.Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation failed: " + ex.Message);
                customerView.Pause();
            }
        }
    }

    private void AddCustomer()
    {
        Console.Clear();

        Console.WriteLine("==========================================");
        Console.WriteLine("              ADD CUSTOMER");
        Console.WriteLine("==========================================");
        Console.WriteLine();

        // ID is checked immediately.
        int id = customerView.ReadCustomerId();

        while (repository.Exists(id))
        {
            Console.WriteLine(
                "Customer ID already exists. Please enter another ID.");

            id = customerView.ReadCustomerId();
        }

        // Read the remaining customer details.
        Customer customer =
            customerView.ReadCustomer(false);

        customer.CustomerId = id;

        // Final validation before saving.
        List<string> errors =
            validator.Validate(
                customer,
                repository,
                true);

        if (errors.Count > 0)
        {
            ShowValidationErrors(errors);
            customerView.Pause();
            return;
        }

        repository.Add(customer);

        Console.WriteLine();
        Console.WriteLine("Customer added successfully.");

        customerView.Pause();
    }

    private void ViewCustomers()
    {
        Console.Clear();

        Console.WriteLine("==========================================");
        Console.WriteLine("              CUSTOMER DETAILS");
        Console.WriteLine("==========================================");

        List<Customer> customers =
            repository.GetAll();

        if (customers.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No customers found.");
        }
        else
        {
            foreach (Customer customer in customers)
            {
                customerView.ShowCustomer(customer);
            }
        }

        customerView.Pause();
    }

    private void UpdateCustomer()
    {
        Console.Clear();

        Console.Write("Enter Customer ID to update: ");

        int id =
            customerView.ReadInt();

        Customer existing =
            repository.GetById(id);

        if (existing == null)
        {
            Console.WriteLine("Customer not found.");
            customerView.Pause();
            return;
        }

        Console.WriteLine("Enter new details.");

        Customer updated =
            customerView.ReadCustomer(false);

        updated.CustomerId = id;

        List<string> errors =
            validator.Validate(
                updated,
                repository,
                false);

        if (errors.Count > 0)
        {
            ShowValidationErrors(errors);
            customerView.Pause();
            return;
        }

        repository.Update(updated);

        Console.WriteLine(
            "Customer updated successfully.");

        customerView.Pause();
    }

    private void DeleteCustomer()
    {
        Console.Clear();

        Console.Write("Enter Customer ID to delete: ");

        int id =
            customerView.ReadInt();

        Customer customer =
            repository.GetById(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            customerView.Pause();
            return;
        }

        customerView.ShowCustomer(customer);

        Console.Write(
            "Delete this customer? (Y/N): ");

        string answer =
            Console.ReadLine();

        if (answer != null &&
            answer.ToUpper() == "Y")
        {
            repository.Delete(id);

            Console.WriteLine(
                "Customer deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }

        customerView.Pause();
    }

    private void Search()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();

            Console.WriteLine("==========================================");
            Console.WriteLine("           SEARCH CUSTOMERS");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Search by Customer ID");
            Console.WriteLine("2. Search by Name");
            Console.WriteLine("3. Search by Email");
            Console.WriteLine("4. Search by Phone");
            Console.WriteLine("5. Search by Customer Type");
            Console.WriteLine("6. Back");
            Console.WriteLine("==========================================");
            Console.Write("Enter choice: ");

            string choice =
                Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SearchById();
                    break;

                case "2":
                    SearchByName();
                    break;

                case "3":
                    SearchByEmail();
                    break;

                case "4":
                    SearchByPhone();
                    break;

                case "5":
                    SearchByCustomerType();
                    break;

                case "6":
                    back = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    customerView.Pause();
                    break;
            }
        }
    }

    private void SearchById()
    {
        Console.Clear();

        Console.Write("Enter Customer ID: ");

        int id =
            customerView.ReadInt();

        Customer customer =
            repository.GetById(id);

        if (customer == null)
        {
            Console.WriteLine(
                "No matching customer found.");
        }
        else
        {
            customerView.ShowCustomer(customer);
        }

        customerView.Pause();
    }

    private void SearchByName()
    {
        Console.Clear();

        Console.Write("Enter customer name: ");

        string text =
            Console.ReadLine();

        List<Customer> customers =
            repository.GetAll();

        List<Customer> results =
            customers
                .Where(c =>
                    c.CustomerName.Contains(
                        text ?? "",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        ShowSearchResults(results);
    }

    private void SearchByEmail()
    {
        Console.Clear();

        Console.Write("Enter email: ");

        string text =
            Console.ReadLine();

        List<Customer> customers =
            repository.GetAll();

        List<Customer> results =
            customers
                .Where(c =>
                    c.Email.Contains(
                        text ?? "",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        ShowSearchResults(results);
    }

    private void SearchByPhone()
    {
        Console.Clear();

        Console.Write("Enter phone number: ");

        string text =
            Console.ReadLine();

        List<Customer> customers =
            repository.GetAll();

        List<Customer> results =
            customers
                .Where(c =>
                    c.PhoneNumber.Contains(
                        text ?? "",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        ShowSearchResults(results);
    }

    private void SearchByCustomerType()
    {
        Console.Clear();

        Console.Write("Enter customer type: ");

        string text =
            Console.ReadLine();

        List<Customer> customers =
            repository.GetAll();

        List<Customer> results =
            customers
                .Where(c =>
                    c.CustomerType.Contains(
                        text ?? "",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        ShowSearchResults(results);
    }

    private void ShowSearchResults(
        List<Customer> results)
    {
        Console.WriteLine();

        if (results.Count == 0)
        {
            Console.WriteLine(
                "No matching customers found.");
        }
        else
        {
            Console.WriteLine("SEARCH RESULTS");
            Console.WriteLine(
                "------------------------------------------");

            foreach (Customer customer in results)
            {
                customerView.ShowCustomer(customer);
            }
        }

        customerView.Pause();
    }

    private void Summary()
    {
        Console.Clear();

        Console.WriteLine("==========================================");
        Console.WriteLine("           CUSTOMER SUMMARY");
        Console.WriteLine("==========================================");

        List<Customer> customers =
            repository.GetAll();

        if (customers.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine(
                "No customer data available.");

            customerView.Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("GENERAL STATISTICS");
        Console.WriteLine("------------------------------------------");

        Console.WriteLine(
            "Total Customers       : " +
            customers.Count);

        DateTime firstDate =
            customers.Min(
                c => c.RegistrationDate);

        DateTime latestDate =
            customers.Max(
                c => c.RegistrationDate);

        Console.WriteLine(
            "First Registration    : " +
            firstDate.ToString("dd-MM-yyyy"));

        Console.WriteLine(
            "Latest Registration   : " +
            latestDate.ToString("dd-MM-yyyy"));

        Console.WriteLine();
        Console.WriteLine("CUSTOMER TYPES");
        Console.WriteLine("------------------------------------------");

        var customerTypes =
            customers
                .GroupBy(c => c.CustomerType)
                .OrderBy(g => g.Key);

        foreach (var type in customerTypes)
        {
            string typeName = type.Key;

            if (string.IsNullOrWhiteSpace(typeName))
            {
                typeName = "Not Specified";
            }

            Console.WriteLine(
                typeName.PadRight(22) +
                ": " + type.Count());
        }

        Console.WriteLine();
        Console.WriteLine("CONTACT INFORMATION");
        Console.WriteLine("------------------------------------------");

        int customersWithEmail =
            customers.Count(
                c => !string.IsNullOrWhiteSpace(c.Email));

        int customersWithPhone =
            customers.Count(
                c => !string.IsNullOrWhiteSpace(c.PhoneNumber));

        Console.WriteLine(
            "With Email            : " +
            customersWithEmail);

        Console.WriteLine(
            "With Phone Number     : " +
            customersWithPhone);

        Console.WriteLine();
        Console.WriteLine(
            "==========================================");

        customerView.Pause();
    }

    private void ImportExport()
    {
        bool back = false;

        while (!back)
        {
            importExportView.Show();

            string choice =
                Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ExportJson();
                        break;

                    case "2":
                        ExportXml();
                        break;

                    case "3":
                        ImportJson();
                        break;

                    case "4":
                        ImportXml();
                        break;

                    case "5":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        customerView.Pause();
                        break;
                }
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine(
                    "Invalid JSON structure. Import cancelled.");

                customerView.Pause();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(
                    "Invalid file data: " +
                    ex.Message);

                customerView.Pause();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "File operation failed: " +
                    ex.Message);

                customerView.Pause();
            }
        }
    }

    private void ExportJson()
    {
        List<Customer> customers =
            repository.GetAll();

        fileService.ExportJson(
            customers,
            jsonPath);

        Console.WriteLine(
            "JSON exported successfully.");

        Console.WriteLine(
            "File: " + jsonPath);

        customerView.Pause();
    }

    private void ExportXml()
    {
        List<Customer> customers =
            repository.GetAll();

        fileService.ExportXml(
            customers,
            xmlPath);

        Console.WriteLine(
            "XML exported successfully.");

        Console.WriteLine(
            "File: " + xmlPath);

        customerView.Pause();
    }

    private void ImportJson()
    {
        if (!File.Exists(jsonPath))
        {
            Console.WriteLine(
                "customers.json was not found.");

            customerView.Pause();
            return;
        }

        List<Customer> customers =
            fileService.ImportJson(jsonPath);

        ImportCustomers(customers);
    }

    private void ImportXml()
    {
        if (!File.Exists(xmlPath))
        {
            Console.WriteLine(
                "customers.xml was not found.");

            customerView.Pause();
            return;
        }

        List<Customer> customers =
            fileService.ImportXml(xmlPath);

        ImportCustomers(customers);
    }

    private void ImportCustomers(
        List<Customer> customers)
    {
        int added = 0;
        int skipped = 0;

        foreach (Customer customer in customers)
        {
            List<string> errors =
                validator.Validate(
                    customer,
                    repository,
                    true);

            if (errors.Count > 0)
            {
                skipped++;
                continue;
            }

            repository.Add(customer);
            added++;
        }

        Console.WriteLine();
        Console.WriteLine("Import completed.");
        Console.WriteLine(
            "Records added   : " + added);
        Console.WriteLine(
            "Records skipped : " + skipped);

        customerView.Pause();
    }

    private void ShowValidationErrors(
        List<string> errors)
    {
        Console.WriteLine();
        Console.WriteLine("Validation errors:");

        foreach (string error in errors)
        {
            Console.WriteLine("- " + error);
        }
    }
}