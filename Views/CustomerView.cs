using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Views;

public class CustomerView
{
    public int ReadCustomerId()
    {
        while (true)
        {
            Console.Write("Customer ID: ");

            string input = Console.ReadLine();

            int id;

            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Customer ID must be a number.");
                continue;
            }

            if (id <= 0)
            {
                Console.WriteLine("Customer ID must be greater than zero.");
                continue;
            }

            return id;
        }
    }

    public Customer ReadCustomer(bool includeId)
    {
        Customer customer = new Customer();

        if (includeId)
        {
            customer.CustomerId = ReadCustomerId();
        }

        customer.CustomerName = ReadCustomerName();
        customer.Email = ReadEmail();
        customer.PhoneNumber = ReadPhoneNumber();
        customer.Address = ReadAddress();
        customer.CustomerType = ReadCustomerType();
        customer.RegistrationDate = ReadRegistrationDate();

        return customer;
    }

    private string ReadCustomerName()
    {
        while (true)
        {
            Console.Write("Customer Name: ");

            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Customer name cannot be empty.");
                continue;
            }

            bool valid = true;

            foreach (char character in name)
            {
                if (!char.IsLetter(character) && character != ' ')
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                Console.WriteLine(
                    "Customer name should contain only letters.");
                continue;
            }

            return name.Trim();
        }
    }

    private string ReadEmail()
    {
        while (true)
        {
            Console.Write("Email Address: ");

            string email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email address cannot be empty.");
                continue;
            }

            if (!email.Contains("@"))
            {
                Console.WriteLine("Email address must contain @.");
                continue;
            }

            return email.Trim();
        }
    }

    private string ReadPhoneNumber()
    {
        while (true)
        {
            Console.Write("Phone Number: ");

            string phone = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(phone))
            {
                Console.WriteLine("Phone number cannot be empty.");
                continue;
            }

            bool valid = true;

            foreach (char character in phone)
            {
                if (!char.IsDigit(character))
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                Console.WriteLine(
                    "Phone number should contain only numbers.");
                continue;
            }

            return phone.Trim();
        }
    }

    private string ReadAddress()
    {
        while (true)
        {
            Console.Write("Address: ");

            string address = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Address cannot be empty.");
                continue;
            }

            return address.Trim();
        }
    }

    private string ReadCustomerType()
    {
        while (true)
        {
            Console.Write("Customer Type: ");

            string customerType = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(customerType))
            {
                Console.WriteLine("Customer type cannot be empty.");
                continue;
            }

            return customerType.Trim();
        }
    }

    private DateTime ReadRegistrationDate()
    {
        while (true)
        {
            Console.Write("Registration Date (yyyy-MM-dd): ");

            string input = Console.ReadLine();

            DateTime date;

            if (!DateTime.TryParse(input, out date))
            {
                Console.WriteLine("Please enter a valid date.");
                continue;
            }

            if (date > DateTime.Now)
            {
                Console.WriteLine(
                    "Registration date cannot be a future date.");
                continue;
            }

            return date;
        }
    }

    public int ReadInt()
    {
        while (true)
        {
            string input = Console.ReadLine();

            int value;

            if (int.TryParse(input, out value))
            {
                return value;
            }

            Console.Write("Invalid number. Try again: ");
        }
    }

    public void ShowCustomer(Customer customer)
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("ID: " + customer.CustomerId);
        Console.WriteLine("Name: " + customer.CustomerName);
        Console.WriteLine("Email: " + customer.Email);
        Console.WriteLine("Phone: " + customer.PhoneNumber);
        Console.WriteLine("Address: " + customer.Address);
        Console.WriteLine("Type: " + customer.CustomerType);
        Console.WriteLine(
            "Registration Date: " +
            customer.RegistrationDate.ToString("yyyy-MM-dd"));
        Console.WriteLine("------------------------------------------");
    }

    public void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}