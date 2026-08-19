using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.ViewModels;

public class SummaryViewModel
{
    private ICustomerService service;

    public int TotalCustomers { get; private set; }

    public int RegularCustomers { get; private set; }

    public int PremiumCustomers { get; private set; }

    public int OtherCustomers { get; private set; }

    public SummaryViewModel(ICustomerService service)
    {
        this.service = service;
    }

    public void Load()
    {
        List<Customer> customers = service.GetAllCustomers();

        TotalCustomers = customers.Count;
        RegularCustomers = 0;
        PremiumCustomers = 0;
        OtherCustomers = 0;

        foreach (Customer customer in customers)
        {
            if (customer.CustomerType.Equals(
                "Regular",
                StringComparison.OrdinalIgnoreCase))
            {
                RegularCustomers++;
            }
            else if (customer.CustomerType.Equals(
                "Premium",
                StringComparison.OrdinalIgnoreCase))
            {
                PremiumCustomers++;
            }
            else
            {
                OtherCustomers++;
            }
        }
    }
}
