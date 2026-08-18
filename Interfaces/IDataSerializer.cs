using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface IDataSerializer
{
    void Export(List<Customer> customers, string filePath);

    List<Customer> Import(string filePath);
}