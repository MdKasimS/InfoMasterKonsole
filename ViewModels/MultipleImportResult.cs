using System.Collections.Generic;
using InfoMasterKonsole.Models;

namespace InfoMasterKonsole.ViewModels
{
    public class FailedFileRecord
    {
        public string FilePath { get; set; }
        public List<string> Errors { get; set; }

        public FailedFileRecord(string filePath, List<string> errors)
        {
            FilePath = filePath;
            Errors = errors;
        }
    }

    public class MultipleImportResult
    {
        public int FilesProcessed { get; set; }
        public int RecordsFound { get; set; }
        public List<Customer> Imported { get; } = new List<Customer>();
        public List<RejectionRecord> Duplicates { get; } = new List<RejectionRecord>();
        public List<RejectionRecord> Rejected { get; } = new List<RejectionRecord>();
        public List<FailedFileRecord> FailedFiles { get; } = new List<FailedFileRecord>();
    }
}
