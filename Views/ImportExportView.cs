using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.ViewModels;

namespace InfoMasterKonsole.Views;

public class ImportExportView
{
    private ICustomerService service;

    public ImportExportView(ICustomerService service)
    {
        this.service = service;
    }

    public void Show()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("        IMPORT / EXPORT DATA");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Export JSON");
            Console.WriteLine("2. Export XML");
            Console.WriteLine("3. Import JSON");
            Console.WriteLine("4. Import XML");
            Console.WriteLine("5. Back");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    ExportJson();
                    break;

                case "2":
                    ExportXml();
                    break;

                case "3":
                    ImportJson();
                    break;

                case "4":
                    ImportXml();
                    break;

                case "5":
                    running = false;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid choice.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void ExportJson()
    {
        ImportExportViewModel viewModel = new ImportExportViewModel(service);

        try
        {
            viewModel.ExportJson();

            Console.WriteLine();
            Console.WriteLine(
                "Customer data exported to JSON successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "JSON export failed.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private void ExportXml()
    {
        ImportExportViewModel viewModel = new ImportExportViewModel(service);

        try
        {
            viewModel.ExportXml();

            Console.WriteLine();
            Console.WriteLine(
                "Customer data exported to XML successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "XML export failed.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private void ImportJson()
    {
        ImportExportViewModel viewModel = new ImportExportViewModel(service);

        try
        {
            viewModel.ImportJson();

            Console.WriteLine();

            if (viewModel.IsSuccess)
            {
                Console.WriteLine(
                    "Customer data imported from JSON successfully.");
            }
            else
            {
                Console.WriteLine(
                    "JSON import completed with errors.");

                foreach (string error in viewModel.ErrorMessages)
                {
                    Console.WriteLine("- " + error);
                }
            }

            if (viewModel.SkippedMessages.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Skipped records:");

                foreach (string skipped in viewModel.SkippedMessages)
                {
                    Console.WriteLine("- " + skipped);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "JSON import failed.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private void ImportXml()
    {
        ImportExportViewModel viewModel = new ImportExportViewModel(service);

        try
        {
            viewModel.ImportXml();

            Console.WriteLine();

            if (viewModel.IsSuccess)
            {
                Console.WriteLine(
                    "Customer data imported from XML successfully.");
            }
            else
            {
                Console.WriteLine(
                    "XML import completed with errors.");

                foreach (string error in viewModel.ErrorMessages)
                {
                    Console.WriteLine("- " + error);
                }
            }

            if (viewModel.SkippedMessages.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Skipped records:");

                foreach (string skipped in viewModel.SkippedMessages)
                {
                    Console.WriteLine("- " + skipped);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine(
                "XML import failed.");

            Console.WriteLine(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}
