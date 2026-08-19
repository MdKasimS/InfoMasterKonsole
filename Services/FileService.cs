using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;
using System.Text.Json;
using System.Xml.Serialization;

namespace InfoMasterKonsole.Services;

public class FileService : IFileService
{
    public void ExportJson(
        List<Customer> customers,
        string path)
    {
        JsonSerializerOptions options =
            new JsonSerializerOptions();

        options.WriteIndented = true;

        string json =
            JsonSerializer.Serialize(
                customers,
                options);

        File.WriteAllText(path, json);
    }

    public List<Customer> ImportJson(
        string path)
    {
        string json =
            File.ReadAllText(path);

        List<Customer> customers =
            JsonSerializer.Deserialize<List<Customer>>(json);

        if (customers == null)
        {
            throw new JsonException(
                "JSON does not contain a valid customer list.");
        }

        return customers;
    }

    public void ExportXml(
        List<Customer> customers,
        string path)
    {
        XmlSerializer serializer =
            new XmlSerializer(
                typeof(List<Customer>));

        using (FileStream stream =
               File.Create(path))
        {
            serializer.Serialize(
                stream,
                customers);
        }
    }

    public List<Customer> ImportXml(
        string path)
    {
        XmlSerializer serializer =
            new XmlSerializer(
                typeof(List<Customer>));

        using (FileStream stream =
               File.OpenRead(path))
        {
            List<Customer> customers =
                (List<Customer>)
                serializer.Deserialize(stream);

            if (customers == null)
            {
                throw new InvalidOperationException(
                    "XML does not contain a valid customer list.");
            }

            return customers;
        }
    }
}