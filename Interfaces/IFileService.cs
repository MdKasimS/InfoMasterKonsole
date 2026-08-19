using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Interfaces;

public interface IFileService
{
    void ExportJson(List<Customer> customers, string path);
    List<Customer> ImportJson(string path);
    void ExportXml(List<Customer> customers, string path);
    List<Customer> ImportXml(string path);
}
