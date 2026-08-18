using System;
using System.IO;
using InfoMasterKonsole.Services;
using InfoMasterKonsole.ViewModels;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Console view for exporting customers to JSON or XML using provided serializers and service.
    /// </summary>
    public class ExportView : IView
    {
        private readonly ICustomerService _service;
        private readonly ICustomerSerializer _jsonSerializer;
        private readonly ICustomerSerializer _xmlSerializer;

        public ExportView(ICustomerService service, ICustomerSerializer jsonSerializer, ICustomerSerializer xmlSerializer)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _xmlSerializer = xmlSerializer ?? throw new ArgumentNullException(nameof(xmlSerializer));
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\nExport Customers");
                Console.WriteLine("1) Export to JSON");
                Console.WriteLine("2) Export to XML");
                Console.WriteLine("0) Back");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        ExportJsonFlow();
                        break;
                    case "2":
                        ExportXmlFlow();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ExportJsonFlow()
        {
            var defaultPath = GetDefaultPath("customers.json");
            Console.Write($"Enter target file path for JSON (leave empty for '{defaultPath}'): ");
            var input = Console.ReadLine();
            var path = string.IsNullOrWhiteSpace(input) ? defaultPath : input.Trim();

            var vm = new ExportViewModel(_service, _jsonSerializer);
            var outcome = vm.ExportAll(path);
            DisplayOutcome(outcome, path, "JSON");
        }

        private void ExportXmlFlow()
        {
            var defaultPath = GetDefaultPath("customers.xml");
            Console.Write($"Enter target file path for XML (leave empty for '{defaultPath}'): ");
            var input = Console.ReadLine();
            var path = string.IsNullOrWhiteSpace(input) ? defaultPath : input.Trim();

            var vm = new ExportViewModel(_service, _xmlSerializer);
            var outcome = vm.ExportAll(path);
            DisplayOutcome(outcome, path, "XML");
        }

        private void DisplayOutcome(ExportOutcome outcome, string path, string format)
        {
            if (outcome == null)
            {
                Console.WriteLine("Export failed: unknown error.");
                return;
            }

            if (outcome.Result.IsSuccess)
            {
                Console.WriteLine($"Export to {format} completed. {outcome.ExportedCount} customers written to '{Path.GetFullPath(path)}'.");
            }
            else
            {
                Console.WriteLine($"Export to {format} failed:");
                foreach (var err in outcome.Result.Errors)
                {
                    Console.WriteLine(" - " + err);
                }
            }
        }

        private string GetDefaultPath(string fileName)
        {
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataDir = Path.Combine(baseDir, "data", "exports");
            if (!Directory.Exists(dataDir))
            {
                try { Directory.CreateDirectory(dataDir); } catch { }
            }
            return Path.Combine(dataDir, fileName);
        }
    }
}
