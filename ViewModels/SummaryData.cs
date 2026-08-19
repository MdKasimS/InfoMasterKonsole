using System;
using System.Collections.Generic;

namespace InfoMasterKonsole.ViewModels
{
    public class SummaryData
    {
        public int TotalCustomers { get; set; }
        public Dictionary<string, int> CustomersByType { get; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public DateTime? EarliestRegistration { get; set; }
        public DateTime? LatestRegistration { get; set; }
    }
}
