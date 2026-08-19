namespace InfoMasterKonsole.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public string CustomerType { get; set; }

    public DateTime RegistrationDate { get; set; }
}