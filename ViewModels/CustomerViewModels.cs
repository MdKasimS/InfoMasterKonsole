using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.ViewModels;

public class CustomerViewModel
{
    public Customer Customer { get; set; }

    public CustomerViewModel()
    {
        Customer = new Customer();
    }

    public CustomerViewModel(Customer customer)
    {
        Customer = customer;
    }
}