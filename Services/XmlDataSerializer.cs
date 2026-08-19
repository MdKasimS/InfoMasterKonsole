using System.Xml.Serialization;
using InfoMasterKonsole.Interfaces;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services;

public class XmlDataSerializer : IDataSerializer
{
    public void Export(List<Customer> customers, string filePath)
    {
        XmlSerializer serializer =
            new XmlSerializer(typeof(List<Customer>));

        using (FileStream stream =
            new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, customers);
        }
    }

    public List<Customer> Import(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "The XML file was not found.");
        }

        XmlSerializer serializer =
            new XmlSerializer(typeof(List<Customer>));

        try
        {
            using (FileStream stream =
                new FileStream(filePath, FileMode.Open))
            {
                List<Customer>? customers =
                    serializer.Deserialize(stream)
                    as List<Customer>;

                if (customers == null)
                {
                    throw new FormatException(
                        "The XML file does not contain valid customer data.");
                }

                return customers;
            }
        }
        catch (InvalidOperationException)
        {
            throw new FormatException(
                "The XML file has an invalid structure.");
        }
    }
}