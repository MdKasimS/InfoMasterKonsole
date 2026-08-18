using System;
using System.Collections.Generic;
using System.Linq;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Coordinates import operations using an ICustomerService and an ICustomerSerializer.
    /// </summary>
    public class ImportViewModel
    {
        private readonly ICustomerService _service;
        private readonly ICustomerSerializer _serializer;

        public ImportViewModel(ICustomerService service, ICustomerSerializer serializer)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public ImportResult Import(string path)
        {
            var outcome = new ImportResult();

            // Deserialize first
            var desResult = _serializer.Deserialize(path, out List<Customer> customers);
            outcome.DeserializeResult = desResult;

            if (!desResult.IsSuccess)
            {
                // Nothing to import
                return outcome;
            }

            if (customers == null || customers.Count == 0)
            {
                return outcome;
            }

            foreach (var c in customers)
            {
                try
                {
                    var addResult = _service.Add(c);
                    if (addResult.IsSuccess)
                    {
                        outcome.Imported.Add(c);
                    }
                    else
                    {
                        // Distinguish duplicates from other validation failures by inspecting error messages
                        var errors = addResult.Errors ?? new List<string>();
                        var isDuplicate = errors.Any(e => e.IndexOf("already exists", StringComparison.OrdinalIgnoreCase) >= 0 || e.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0);
                        if (isDuplicate)
                        {
                            outcome.Duplicates.Add(new RejectionRecord(c, errors));
                        }
                        else
                        {
                            outcome.Rejected.Add(new RejectionRecord(c, errors));
                        }
                    }
                }
                catch (Exception ex)
                {
                    InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                    outcome.Rejected.Add(new RejectionRecord(c, new List<string> { "Unexpected error while importing this record. See logs for details." }));
                }
            }

            return outcome;
        }
    }
}
