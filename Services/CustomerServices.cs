using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Validation;

namespace InfoMasterKonsole.Services;

public class CustomerService : ICustomerService
{
    private ICustomerRepository repository;
    private CustomerValidator validator;
    private IDataSerializer jsonSerializer;
    private IDataSerializer xmlSerializer;

    public CustomerService(
    ICustomerRepository repository,
    CustomerValidator validator,
    IDataSerializer jsonSerializer,
    IDataSerializer xmlSerializer)
    {
        this.repository = repository;
        this.validator = validator;
        this.jsonSerializer = jsonSerializer;
        this.xmlSerializer = xmlSerializer;
    }

    public bool AddCustomer(Customer customer, out List<string> errors)
    {
        if (!validator.Validate(customer, false, out errors))
        {
            return false;
        }

        repository.Add(customer);
        return true;
    }

    public List<Customer> GetAllCustomers()
    {
        return repository.GetAll();
    }

    public Customer? GetCustomerById(int customerId)
    {
        return repository.GetById(customerId);
    }

    public List<Customer> SearchCustomers(string searchText)
    {
        return repository.Search(searchText);
    }

    public bool UpdateCustomer(Customer customer, out List<string> errors)
    {
        if (!repository.Exists(customer.CustomerId))
        {
            errors = new List<string>();
            errors.Add("Customer does not exist.");
            return false;
        }

        if (!validator.Validate(customer, true, out errors))
        {
            return false;
        }

        repository.Update(customer);
        return true;
    }

    public bool DeleteCustomer(int customerId)
    {
        if (!repository.Exists(customerId))
        {
            return false;
        }

        repository.Delete(customerId);
        return true;
    }

    public int GetCustomerCount()
    {
        return repository.Count();
    }

    public void ExportJson(string filePath)
    {
        List<Customer> customers = repository.GetAll();

        jsonSerializer.Export(customers, filePath);
    }

    public void ExportXml(string filePath)
    {
        List<Customer> customers = repository.GetAll();

        xmlSerializer.Export(customers, filePath);
    }

    public bool ImportJson(
    string filePath,
    out List<string> errors,
    out List<string> skippedMessages)
    {
        errors = new List<string>();
        skippedMessages = new List<string>();

        List<Customer> customers;

        try
        {
            customers = jsonSerializer.Import(filePath);
        }
        catch (FileNotFoundException exception)
        {
            errors.Add(exception.Message);
            return false;
        }
        catch (FormatException exception)
        {
            errors.Add(exception.Message);
            return false;
        }

        ImportCustomers(customers, errors, skippedMessages);

        return errors.Count == 0;
    }

    public bool ImportXml(
        string filePath,
        out List<string> errors,
        out List<string> skippedMessages)
    {
        errors = new List<string>();
        skippedMessages = new List<string>();

        List<Customer> customers;

        try
        {
            customers = xmlSerializer.Import(filePath);
        }
        catch (FileNotFoundException exception)
        {
            errors.Add(exception.Message);
            return false;
        }
        catch (FormatException exception)
        {
            errors.Add(exception.Message);
            return false;
        }

        ImportCustomers(customers, errors, skippedMessages);

        return errors.Count == 0;
    }

    // Shared by ImportJson and ImportXml. For each customer read from
    // the file: an ID that already exists in the database is treated
    // as a skip (not an error), invalid field values are treated as
    // an error, and everything else gets added to the repository.
    private void ImportCustomers(
        List<Customer> customers,
        List<string> errors,
        List<string> skippedMessages)
    {
        foreach (Customer customer in customers)
        {
            if (repository.Exists(customer.CustomerId))
            {
                skippedMessages.Add(
                    "Customer ID " + customer.CustomerId +
                    " already exists - skipped.");

                continue;
            }

            if (!validator.ValidateImportedCustomer(
                customer,
                out List<string> customerErrors))
            {
                errors.AddRange(customerErrors);
                continue;
            }

            repository.Add(customer);
        }
    }







}