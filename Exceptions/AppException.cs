using System;

namespace InfoMasterKonsole.Exceptions
{
    /// <summary>
    /// Base application exception for Phase 1. Specific exception types will be added later.
    /// </summary>
    public class AppException : Exception
    {
        public AppException() { }
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception inner) : base(message, inner) { }
    }
}
