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
            _xmlSerializer = xmlSerializer ?? throw new ArgumentNullException(nameof(xmlSerializer));
        }

        public void Show()
        {
            while (true)
            {
                ScreenHelper.ShowTitle("Import Customers");
                Console.WriteLine("1) Import from JSON");
                Console.WriteLine("2) Import from XML");
                Console.WriteLine("3) Import multiple JSON files from folder");
                Console.WriteLine("0) Back");
                Console.Write("Select option: ");
                var choice = Console.ReadLine();

                if (choice == "0")
                {
                    Console.Clear();
                    return;
                }

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        ImportJsonFlow();
                        ScreenHelper.PauseAndClear();
                        break;
                    case "2":
                        Console.Clear();
                        ImportXmlFlow();
                        ScreenHelper.PauseAndClear();
                        break;
                    case "3":
                        Console.Clear();
                        ImportMultipleJsonFlow();
                        ScreenHelper.PauseAndClear();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        ScreenHelper.PauseAndClear();
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

        private void ImportMultipleJsonFlow()
        {
            var defaultFolder = GetDefaultFolder();
            Console.Write($"Enter folder path containing JSON files (leave empty for '{defaultFolder}'): ");
            var input = Console.ReadLine();
            var folder = string.IsNullOrWhiteSpace(input) ? defaultFolder : input.Trim();

            var vm = new ImportViewModel(_service, _jsonSerializer);
            var result = vm.ImportMultipleJsonFolder(folder);
            DisplayMultipleImportResult(result, folder);
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
                try { Directory.CreateDirectory(dataDir); } catch (Exception ex) { InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex); }
            }
            return Path.Combine(dataDir, fileName);
        }

        private string GetDefaultFolder()
        {
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataDir = Path.Combine(baseDir, "data", "imports");
            if (!Directory.Exists(dataDir))
            {
                try { Directory.CreateDirectory(dataDir); } catch (Exception ex) { InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex); }
            }
            return dataDir;
        }

        private void DisplayMultipleImportResult(ViewModels.MultipleImportResult result, string folder)
        {
            if (result == null)
            {
                Console.WriteLine("Import failed: unknown error.");
                return;
            }

            Console.WriteLine($"Import summary for folder '{folder}':");
            Console.WriteLine($"Files processed: {result.FilesProcessed}");
            Console.WriteLine($"Records found: {result.RecordsFound}");
            Console.WriteLine($"Imported: {result.Imported.Count}");
            Console.WriteLine($"Duplicate records: {result.Duplicates.Count}");
            Console.WriteLine($"Invalid records: {result.Rejected.Count}");
            Console.WriteLine($"Failed files: {result.FailedFiles.Count}");

            if (result.FailedFiles.Count > 0)
            {
                Console.WriteLine("\nFailed files:");
                foreach (var f in result.FailedFiles)
                {
                    Console.WriteLine($" - {f.FilePath}");
                    foreach (var e in f.Errors) Console.WriteLine("   * " + e);
                }
            }

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
                Console.WriteLine("\nInvalid records:");
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
    }
}
