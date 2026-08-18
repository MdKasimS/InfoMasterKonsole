using System;
using System.Collections.Generic;
using System.Linq;
using InfoMasterKonsole.Models;
using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Coordinates export operations using an ICustomerService and ICustomerSerializer.
    /// </summary>
    public class ExportViewModel
    {
        private readonly ICustomerService _service;
        private readonly ICustomerSerializer _serializer;

        public ExportViewModel(ICustomerService service, ICustomerSerializer serializer)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public ExportOutcome ExportAll(string path)
        {
            var outcome = new ExportOutcome();
            try
            {
                var customers = _service.GetAll() ?? Enumerable.Empty<Customer>();
                var list = new List<Customer>(customers);
                var result = _serializer.Serialize(path, list);
                outcome.Result = result;
                if (result.IsSuccess)
                {
                    outcome.ExportedCount = list.Count;
                }
                else
                {
                    outcome.ExportedCount = 0;
                }
            }
            catch (Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                outcome.Result = ServiceResult.Failure("Unexpected error during export. See logs for details.");
                outcome.ExportedCount = 0;
            }

            return outcome;
        }
    }
}
