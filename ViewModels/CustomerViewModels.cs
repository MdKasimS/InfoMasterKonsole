using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.ViewModels;

// Backs the Add/View/Update/Delete customer screens. Holds the customer
// being worked on plus the outcome of the last operation (success flag
// and error messages) so the View can render it without talking to
// ICustomerService directly.
public class CustomerViewModel
{
    private ICustomerService service;

    public Customer Customer { get; set; }

    public List<Customer> Customers { get; private set; }

    public List<string> ErrorMessages { get; private set; }

    public bool IsSuccess { get; private set; }

    public CustomerViewModel(ICustomerService service)
    {
        this.service = service;
        Customer = new Customer();
        Customers = new List<Customer>();
        ErrorMessages = new List<string>();
    }

    public void Add()
    {
        List<string> errors;

        IsSuccess = service.AddCustomer(Customer, out errors);
        ErrorMessages = errors;
    }

    // Loads a customer into Customer for viewing/editing.
    // Returns false if no customer with that ID exists.
    public bool Load(int customerId)
    {
        Customer? existing = service.GetCustomerById(customerId);

        if (existing == null)
        {
            Customer = new Customer();
            return false;
        }

        Customer = existing;
        return true;
    }

    public void Update()
    {
        List<string> errors;

        IsSuccess = service.UpdateCustomer(Customer, out errors);
        ErrorMessages = errors;
    }

    public bool Delete(int customerId)
    {
        IsSuccess = service.DeleteCustomer(customerId);
        return IsSuccess;
    }

    public void LoadAll()
    {
        Customers = service.GetAllCustomers();
    }
}