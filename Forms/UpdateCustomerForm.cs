using System;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Forms
{
    /// <summary>
    /// Collects updated customer information from console. The provided existing customer identifies which record
    /// to update; its Id is not changed here.
    /// </summary>
    public class UpdateCustomerForm
    {
        public Customer UpdateCustomerFromConsole(Customer existing)
        {
            if (existing == null) throw new ArgumentNullException(nameof(existing));

            Console.WriteLine($"Updating customer ID: {existing.Id}");

            Console.Write($"Customer Name [{existing.Name}]: ");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) existing.Name = name;

            Console.Write($"Email [{existing.Email}]: ");
            var email = Console.ReadLine();
            if (!string.IsNullOrEmpty(email)) existing.Email = email;

            Console.Write($"Phone Number [{existing.Phone}]: ");
            var phone = Console.ReadLine();
            if (!string.IsNullOrEmpty(phone)) existing.Phone = phone;

            Console.Write($"Address [{existing.Address}]: ");
            var address = Console.ReadLine();
            if (!string.IsNullOrEmpty(address)) existing.Address = address;

            Console.Write($"Customer Type [{existing.CustomerType}]: ");
            var ctype = Console.ReadLine();
            if (!string.IsNullOrEmpty(ctype)) existing.CustomerType = ctype;

            // Registration date - allow empty to keep existing
            existing.RegistrationDate = PromptForDateAllowEmpty($"Registration Date [{existing.RegistrationDate:yyyy-MM-dd}]: ", existing.RegistrationDate);

            return existing;
        }

        private DateTime PromptForDateAllowEmpty(string prompt, DateTime current)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return current.Date;
                }

                if (DateTime.TryParse(input, out var parsed))
                {
                    return parsed.Date;
                }

                Console.WriteLine("Invalid date format. Please try again or leave empty to keep the current date.");
            }
        }
    }
}
