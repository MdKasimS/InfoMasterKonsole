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

        public MultipleImportResult ImportMultipleJsonFolder(string folderPath)
        {
            var result = new MultipleImportResult();

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                result.FailedFiles.Add(new FailedFileRecord(folderPath ?? string.Empty, new List<string> { "Invalid folder path." }));
                return result;
            }

            if (!System.IO.Directory.Exists(folderPath))
            {
                result.FailedFiles.Add(new FailedFileRecord(folderPath, new List<string> { "Folder not found." }));
                return result;
            }

            string[] files;
            try
            {
                files = System.IO.Directory.GetFiles(folderPath, "*.json");
            }
            catch (System.Exception ex)
            {
                InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                result.FailedFiles.Add(new FailedFileRecord(folderPath, new List<string> { "Failed to enumerate files in folder." }));
                return result;
            }

            result.FilesProcessed = files.Length;

            var combined = new List<Customer>();

            foreach (var f in files)
            {
                try
                {
                    var des = _serializer.Deserialize(f, out List<Customer> customers);
                    if (!des.IsSuccess)
                    {
                        result.FailedFiles.Add(new FailedFileRecord(f, new List<string>(des.Errors)));
                        continue;
                    }

                    if (customers != null && customers.Count > 0)
                    {
                        combined.AddRange(customers);
                        result.RecordsFound += customers.Count;
                    }
                }
                catch (System.Exception ex)
                {
                    InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                    result.FailedFiles.Add(new FailedFileRecord(f, new List<string> { "Unexpected error reading file. See logs." }));
                }
            }

            // Now process combined customers, detect duplicates within import and against existing DB
            var seenIds = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

            foreach (var c in combined)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(c?.Id))
                    {
                        result.Rejected.Add(new RejectionRecord(c, new List<string> { "Missing or empty Customer ID." }));
                        continue;
                    }

                    if (seenIds.Contains(c.Id))
                    {
                        result.Duplicates.Add(new RejectionRecord(c, new List<string> { "Duplicate ID in imported files." }));
                        continue;
                    }

                    // Check if exists in DB
                    var exists = _service.GetById(c.Id) != null;
                    if (exists)
                    {
                        result.Duplicates.Add(new RejectionRecord(c, new List<string> { "Customer ID already exists in database." }));
                        seenIds.Add(c.Id);
                        continue;
                    }

                    // Try to add via service (performs validation)
                    var addRes = _service.Add(c);
                    if (addRes.IsSuccess)
                    {
                        result.Imported.Add(c);
                        seenIds.Add(c.Id);
                    }
                    else
                    {
                        var errors = addRes.Errors ?? new List<string>();
                        // classify duplicates by message
                        var isDuplicate = errors.Any(e => e.IndexOf("already exists", System.StringComparison.OrdinalIgnoreCase) >= 0 || e.IndexOf("duplicate", System.StringComparison.OrdinalIgnoreCase) >= 0);
                        if (isDuplicate)
                        {
                            result.Duplicates.Add(new RejectionRecord(c, errors));
                            seenIds.Add(c.Id);
                        }
                        else
                        {
                            result.Rejected.Add(new RejectionRecord(c, errors));
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    InfoMasterKonsole.Exceptions.ExceptionLogger.Log(ex);
                    result.Rejected.Add(new RejectionRecord(c, new List<string> { "Unexpected error while processing record. See logs for details." }));
                }
            }

            return result;
        }
    }
}
