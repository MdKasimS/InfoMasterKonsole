using System.Collections.Generic;

namespace InfoMasterKonsole.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public List<string> Errors { get; } = new List<string>();
    }
}
