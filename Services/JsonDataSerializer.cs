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
        string json = File.ReadAllText(filePath);

        List<Customer>? customers =
            JsonSerializer.Deserialize<List<Customer>>(json);

        if (customers == null)
        {
            return new List<Customer>();
        }

        return customers;
    }
}