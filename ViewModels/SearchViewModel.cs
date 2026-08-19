using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.ViewModels;

public class SearchViewModel
{
    private ICustomerService service;

    public List<Customer> Results { get; private set; }

    public SearchViewModel(ICustomerService service)
    {
        this.service = service;
        Results = new List<Customer>();
    }

    public void Search(string searchText)
    {
        Results = service.SearchCustomers(searchText);
    }
}
