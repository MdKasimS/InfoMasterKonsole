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
    out List<string> errors)
    {
        errors = new List<string>();

        List<Customer> customers;

        try
        {
            customers = jsonSerializer.Import(filePath);
        }
        catch (Exception)
        {
            errors.Add(
                "The JSON file is invalid or could not be read.");
            return false;
        }

        foreach (Customer customer in customers)
        {
            if (!validator.ValidateImportedCustomer(
                customer,
                out List<string> customerErrors))
            {
                errors.AddRange(customerErrors);
                continue;
            }

            repository.Add(customer);
        }

        return errors.Count == 0;
    }

    public bool ImportXml(
        string filePath,
        out List<string> errors)
    {
        errors = new List<string>();

        List<Customer> customers;

        try
        {
            customers = xmlSerializer.Import(filePath);
        }
        catch (Exception)
        {
            errors.Add(
                "The XML file is invalid or could not be read.");
            return false;
        }

        foreach (Customer customer in customers)
        {
            if (!validator.ValidateImportedCustomer(
                customer,
                out List<string> customerErrors))
            {
                errors.AddRange(customerErrors);
                continue;
            }

            repository.Add(customer);
        }

        return errors.Count == 0;
    }
    

    

    


}