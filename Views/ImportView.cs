using System;
using System.Collections.Generic;
using System.IO;
using InfoMasterKonsole.Services;
using InfoMasterKonsole.ViewModels;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Console view for importing customers from JSON or XML files.
    /// </summary>
    public class ImportView : IView
    {
        private readonly ICustomerService _service;
        private readonly ICustomerSerializer _jsonSerializer;
        private readonly ICustomerSerializer _xmlSerializer;

        public ImportView(ICustomerService service, ICustomerSerializer jsonSerializer, ICustomerSerializer xmlSerializer)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _xml_serializer: // placeholder
            _xmlSerializer = xmlSerializer ?? throw new ArgumentNullException(nameof(xmlSerializer));
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\nImport Customers");
                Console.WriteLine("1) Import from JSON");
                Console.WriteLine("2) Import from XML");
                Console.WriteLine("0) Back");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        ImportJsonFlow();
                        break;
                    case "2":
                        ImportXmlFlow();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void ImportJsonFlow()
        {
            var defaultPath = GetDefaultPath("customers.json");
            Console.Write($"Enter source file path for JSON (leave empty for '{defaultPath}'): ");
            var input = Console.ReadLine();
            var path = string.IsNullOrWhiteSpace(input) ? defaultPath : input.Trim();

            var vm = new ImportViewModel(_service, _jsonSerializer);
            var result = vm.Import(path);
            DisplayResult(result, path, "JSON");
        }

        private void ImportXmlFlow()
        {
            var defaultPath = GetDefaultPath("customers.xml");
            Console.Write($"Enter source file path for XML (leave empty for '{defaultPath}'): ");
            var input = Console.ReadLine();
            var path = string.IsNullOrWhiteSpace(input) ? defaultPath : input.Trim();

            var vm = new ImportViewModel(_service, _xmlSerializer);
            var result = vm.Import(path);
            DisplayResult(result, path, "XML");
        }

        private void DisplayResult(ImportResult result, string path, string format)
        {
            if (result == null)
            {
                Console.WriteLine("Import failed: unknown error.");
                return;
            }

            if (!result.DeserializeResult.IsSuccess)
            {
                Console.WriteLine($"Failed to read {format} file '{path}':");
                foreach (var err in result.DeserializeResult.Errors)
                {
                    Console.WriteLine(" - " + err);
                }
                return;
            }

            Console.WriteLine($"Import summary for '{Path.GetFullPath(path)}':");
            Console.WriteLine($"Imported: {result.Imported.Count}");
            Console.WriteLine($"Duplicates: {result.Duplicates.Count}");
            Console.WriteLine($"Rejected: {result.Rejected.Count}");

            if (result.Duplicates.Count > 0)
            {
                Console.WriteLine("\nDuplicate records:");
                foreach (var d in result.Duplicates)
                {
                    Console.WriteLine($" - ID: {d.Customer.Id}, Name: {d.Customer.Name}");
                    foreach (var e in d.Errors) Console.WriteLine("   * " + e);
                }
            }

            if (result.Rejected.Count > 0)
            {
                Console.WriteLine("\nRejected records:");
                foreach (var r in result.Rejected)
                {
                    Console.WriteLine($" - ID: {r.Customer.Id}, Name: {r.Customer.Name}");
                    foreach (var e in r.Errors) Console.WriteLine("   * " + e);
                }
            }

            if (result.Imported.Count > 0)
            {
                Console.WriteLine("\nImported records:");
                foreach (var i in result.Imported)
                {
                    Console.WriteLine($" - ID: {i.Id}, Name: {i.Name}");
                }
            }
        }

        private string GetDefaultPath(string fileName)
        {
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataDir = Path.Combine(baseDir, "data", "imports");
            if (!Directory.Exists(dataDir))
            {
                try { Directory.CreateDirectory(dataDir); } catch { }
            }
            return Path.Combine(dataDir, fileName);
        }
    }
}
