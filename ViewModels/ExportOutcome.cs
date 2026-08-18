using InfoMasterKonsole.Services;

namespace InfoMasterKonsole.ViewModels
{
    /// <summary>
    /// Outcome of an export operation, including service result and exported count.
    /// </summary>
    public class ExportOutcome
    {
        public ServiceResult Result { get; set; }
        public int ExportedCount { get; set; }

        public ExportOutcome()
        {
            Result = ServiceResult.Failure("Not executed");
            ExportedCount = 0;
        }
    }
}
