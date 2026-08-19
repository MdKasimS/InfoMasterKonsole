using System;
using System.Linq;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Builds a summary/statistics view from customer data using ICustomerService.
    /// </summary>
    public class SummaryViewModel
    {
        private readonly ICustomerService _service;

        public SummaryViewModel(ICustomerService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public SummaryData GetSummary()
        {
            var summary = new SummaryData();

            var customers = _service.GetAll();
            if (customers == null)
            {
                summary.TotalCustomers = 0;
                return summary;
            }

            var list = customers as System.Collections.Generic.List<InfoMasterKonsole.Models.Customer> ?? new System.Collections.Generic.List<InfoMasterKonsole.Models.Customer>(customers);

            summary.TotalCustomers = list.Count;

            if (list.Count == 0)
            {
                return summary;
            }

            // Customers by type
            foreach (var c in list)
            {
                var type = string.IsNullOrWhiteSpace(c.CustomerType) ? "(Unspecified)" : c.CustomerType.Trim();
                if (summary.CustomersByType.ContainsKey(type))
                {
                    summary.CustomersByType[type]++;
                }
                else
                {
                    summary.CustomersByType[type] = 1;
                }
            }

            // Earliest and latest registration
            try
            {
                var dates = list.Where(c => c != null).Select(c => c.RegistrationDate).ToList();
                if (dates.Count > 0)
                {
                    summary.EarliestRegistration = dates.Min();
                    summary.LatestRegistration = dates.Max();
                }
            }
            catch
            {
                // Keep nulls on error
                summary.EarliestRegistration = null;
                summary.LatestRegistration = null;
            }

            return summary;
        }
    }
}
