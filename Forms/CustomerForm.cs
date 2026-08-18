using System;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Forms
{
    /// <summary>
    /// Collects input needed to create a Customer.
    /// Responsible only for console input and basic parsing (date). No validation or persistence here.
    /// </summary>
    public class CustomerForm
    {
        public Customer CreateCustomerFromConsole()
        {
            var customer = new Customer();

            Console.Write("Customer ID: ");
            customer.Id = Console.ReadLine() ?? string.Empty;

            Console.Write("Customer Name: ");
            customer.Name = Console.ReadLine();

            Console.Write("Email: ");
            customer.Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Phone Number: ");
            customer.Phone = Console.ReadLine();

            Console.Write("Address: ");
            customer.Address = Console.ReadLine();

            Console.Write("Customer Type: ");
            customer.CustomerType = Console.ReadLine();

            customer.RegistrationDate = PromptForDate("Registration Date (yyyy-MM-dd) [leave empty for today]: ");

            return customer;
        }

        private DateTime PromptForDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return DateTime.Today;
                }

                if (DateTime.TryParse(input, out var parsed))
                {
                    return parsed.Date;
                }

                Console.WriteLine("Invalid date format. Please try again or leave empty to use today's date.");
            }
        }
    }
}
