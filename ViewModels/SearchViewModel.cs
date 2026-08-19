using System;
using System.Collections.Generic;
using System.Linq;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Coordinates search operations. Translates simple criteria into predicates
    /// and delegates execution to the ICustomerService.Search method.
    /// </summary>
    public class SearchViewModel
    {
        private readonly ICustomerService _service;

        public SearchViewModel(ICustomerService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Executes a search based on the provided criterion key and value.
        /// Criterion should be one of: "Id", "Name", "Email", "Phone", "CustomerType".
        /// Returns matching customers or an empty enumerable.
        /// </summary>
        public IEnumerable<Customer> Search(string criterion, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Enumerable.Empty<Customer>();
            if (string.IsNullOrWhiteSpace(criterion)) return Enumerable.Empty<Customer>();

            criterion = criterion.Trim();
            value = value.Trim();

            Func<Customer, bool> predicate = criterion switch
            {
                "Id" => (c) => string.Equals(c.Id, value, StringComparison.OrdinalIgnoreCase),
                "Name" => (c) => !string.IsNullOrEmpty(c.Name) && c.Name.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0,
                "Email" => (c) => !string.IsNullOrEmpty(c.Email) && c.Email.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0,
                "Phone" => (c) => !string.IsNullOrEmpty(c.Phone) && c.Phone.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0,
                "CustomerType" => (c) => !string.IsNullOrEmpty(c.CustomerType) && c.CustomerType.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0,
                _ => (c) => false,
            };

            return _service.Search(predicate) ?? Enumerable.Empty<Customer>();
        }
    }
}
