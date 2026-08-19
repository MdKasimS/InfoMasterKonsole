using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface ICustomerValidator
{
    List<string> Validate(Customer customer, ICustomerRepository repository, bool checkId);
}
