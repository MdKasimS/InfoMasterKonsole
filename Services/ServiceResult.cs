using System.Collections.Generic;

namespace InfoMasterKonsole.Services
{
    /// <summary>
    /// Simple operation result for service methods.
    /// </summary>
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; } = new List<string>();

        public static ServiceResult Success()
        {
            return new ServiceResult { IsSuccess = true };
        }

        public static ServiceResult Failure(params string[] errors)
        {
            var r = new ServiceResult { IsSuccess = false };
            if (errors != null)
            {
                r.Errors.AddRange(errors);
            }
            return r;
        }
    }
}
