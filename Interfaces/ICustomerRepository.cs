using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface ICustomerRepository
{
    void Add(Customer customer);
    List<Customer> GetAll();
    Customer GetById(int id);
    bool Exists(int id);
    void Update(Customer customer);
    void Delete(int id);
}
