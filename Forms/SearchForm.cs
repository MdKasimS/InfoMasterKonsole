using System;

namespace InfoMasterKonsole.Forms
{
    /// <summary>
    /// Collects search criterion and value from console input.
    /// Returns a tuple of (criterionKey, value) where criterionKey is one of:
    /// "Id", "Name", "Email", "Phone", "CustomerType".
    /// </summary>
    public class SearchForm
    {
        public (string Criterion, string Value) CollectSearchCriteria()
        {
            Console.WriteLine("Select search criterion:");
            Console.WriteLine("1) Customer ID");
            Console.WriteLine("2) Customer Name");
            Console.WriteLine("3) Email");
            Console.WriteLine("4) Phone Number");
            Console.WriteLine("5) Customer Type");
            Console.Write("Choice (1-5): ");

            var choice = Console.ReadLine();
            string key = choice switch
            {
                "1" => "Id",
                "2" => "Name",
                "3" => "Email",
                "4" => "Phone",
                "5" => "CustomerType",
                _ => "Name"
            };

            Console.Write($"Enter search value for {key}: ");
            var value = Console.ReadLine() ?? string.Empty;

            return (key, value);
        }
    }
}
