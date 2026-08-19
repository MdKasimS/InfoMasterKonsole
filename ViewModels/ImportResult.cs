using System.Collections.Generic;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    public class RejectionRecord
    {
        public Customer Customer { get; set; }
        public List<string> Errors { get; set; }

        public RejectionRecord(Customer customer, List<string> errors)
        {
            Customer = customer;
            Errors = errors;
        }
    }

    public class ImportResult
    {
        public ServiceResult DeserializeResult { get; set; }
        public List<Customer> Imported { get; } = new List<Customer>();
        public List<RejectionRecord> Rejected { get; } = new List<RejectionRecord>();
        public List<RejectionRecord> Duplicates { get; } = new List<RejectionRecord>();

        public ImportResult()
        {
            DeserializeResult = ServiceResult.Failure("Not executed");
        }
    }
}
