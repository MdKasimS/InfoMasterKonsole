using InfoMasterKonsole;

AppManager app = AppManager.GetInstance();

string jsonPath = "Data/customers.json";
string xmlPath = "Data/customers.xml";

Console.WriteLine("Exporting customer data...");
Console.WriteLine();

try
{
    app.CustomerService.ExportJson(jsonPath);
    Console.WriteLine("JSON export successful.");
}
catch (Exception exception)
{
    Console.WriteLine("JSON export failed.");
    Console.WriteLine(exception.Message);
}

try
{
    app.CustomerService.ExportXml(xmlPath);
    Console.WriteLine("XML export successful.");
}
catch (Exception exception)
{
    Console.WriteLine("XML export failed.");
    Console.WriteLine(exception.Message);
}

Console.WriteLine();
Console.WriteLine("Export test complete.");