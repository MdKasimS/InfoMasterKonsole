using System.Collections.Generic;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// Interface for import/export service. Implementations for JSON/XML will be added in later phases.
    /// </summary>
    public interface IImportExportService
    {
        IEnumerable<Customer> ImportFromJson(string path);
        IEnumerable<Customer> ImportFromXml(string path);
        void ExportToJson(string path, IEnumerable<Customer> customers);
        void ExportToXml(string path, IEnumerable<Customer> customers);
    }
}
