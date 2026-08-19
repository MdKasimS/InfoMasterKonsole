using InfoMasterKonsole.Interfaces;

namespace InfoMasterKonsole.ViewModels;

public class ImportExportViewModel
{
    private ICustomerService service;

    public List<string> ErrorMessages { get; private set; }

    public List<string> SkippedMessages { get; private set; }

    public bool IsSuccess { get; private set; }

    public ImportExportViewModel(ICustomerService service)
    {
        this.service = service;
        ErrorMessages = new List<string>();
        SkippedMessages = new List<string>();
    }

    public void ExportJson()
    {
        service.ExportJson(GetDataFilePath("customers.json"));
    }

    public void ExportXml()
    {
        service.ExportXml(GetDataFilePath("customers.xml"));
    }

    public void ImportJson()
    {
        List<string> errors;
        List<string> skippedMessages;

        IsSuccess = service.ImportJson(
            GetDataFilePath("customers.json"),
            out errors,
            out skippedMessages);

        ErrorMessages = errors;
        SkippedMessages = skippedMessages;
    }

    public void ImportXml()
    {
        List<string> errors;
        List<string> skippedMessages;

        IsSuccess = service.ImportXml(
            GetDataFilePath("customers.xml"),
            out errors,
            out skippedMessages);

        ErrorMessages = errors;
        SkippedMessages = skippedMessages;
    }

    private string GetDataFilePath(string fileName)
    {
        // AppContext.BaseDirectory points at the build output folder,
        // e.g. <project>/bin/Debug/net10.0/
        // Three steps up from there lands back on the project folder,
        // regardless of where the application was launched from.
        DirectoryInfo currentDirectory =
            new DirectoryInfo(AppContext.BaseDirectory);

        int stepsToProjectFolder = 3;

        for (int i = 0; i < stepsToProjectFolder; i++)
        {
            if (currentDirectory.Parent == null)
            {
                throw new DirectoryNotFoundException(
                    "Unable to locate the project directory from " +
                    "the application base directory.");
            }

            currentDirectory = currentDirectory.Parent;
        }

        string dataDirectory =
            Path.Combine(
                currentDirectory.FullName,
                "Data");

        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        return Path.Combine(
            dataDirectory,
            fileName);
    }
}