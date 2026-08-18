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
        XmlSerializer serializer =
            new XmlSerializer(typeof(List<Customer>));

        using (FileStream stream =
            new FileStream(filePath, FileMode.Open))
        {
            List<Customer>? customers =
                serializer.Deserialize(stream) as List<Customer>;

            if (customers == null)
            {
                return new List<Customer>();
            }

            return customers;
        }
    }
}