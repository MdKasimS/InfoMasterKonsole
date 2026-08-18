using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface ICustomerRepository
{
    void Add(Customer customer);

    List<Customer> GetAll();

    Customer GetById(int customerId);

    List<Customer> Search(string searchText);

    void Update(Customer customer);

    void Delete(int customerId);

    bool Exists(int customerId);

    int Count();
}