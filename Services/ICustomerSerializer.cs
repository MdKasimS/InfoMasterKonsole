using System.Collections.Generic;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// Serializer contract for Customer collections.
    /// Implementations must not perform validation or persistence to database.
    /// </summary>
    public interface ICustomerSerializer
    {
        /// <summary>
        /// Serialize provided customers to the target file path. Returns ServiceResult indicating success or failure.
        /// </summary>
        ServiceResult Serialize(string path, IEnumerable<Customer> customers);

        /// <summary>
        /// Deserialize customers from the specified file path into the out parameter.
        /// Returns a ServiceResult indicating success or failure. 
        /// On failure, the out parameter will be an empty list.
        /// </summary>
        ServiceResult Deserialize(string path, out List<Customer> customers);
    }
}
