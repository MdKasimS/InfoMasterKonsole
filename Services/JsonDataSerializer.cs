using System.Text.Json;
using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services;

public class JsonDataSerializer : IDataSerializer
{
    public void Export(List<Customer> customers, string filePath)
    {
        string json = JsonSerializer.Serialize(
            customers,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(filePath, json);
    }

    public List<Customer> Import(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "The JSON file was not found.");
        }

        string json = File.ReadAllText(filePath);

        List<Customer>? customers;

        try
        {
            customers =
                JsonSerializer.Deserialize<List<Customer>>(json);
        }
        catch (JsonException)
        {
            throw new FormatException(
                "The JSON file has an invalid structure.");
        }

        if (customers == null)
        {
            throw new FormatException(
                "The JSON file does not contain valid customer data.");
        }

        return customers;
    }
}