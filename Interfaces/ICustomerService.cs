using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface ICustomerService
{
    bool AddCustomer(Customer customer, out List<string> errors);

    List<Customer> GetAllCustomers();

    Customer? GetCustomerById(int customerId);

    List<Customer> SearchCustomers(string searchText);

    bool UpdateCustomer(Customer customer, out List<string> errors);

    bool DeleteCustomer(int customerId);

    int GetCustomerCount();

    void ExportJson(string filePath);

    void ExportXml(string filePath);

    bool ImportJson(string filePath, out List<string> errors);

    bool ImportXml(string filePath, out List<string> errors);
}