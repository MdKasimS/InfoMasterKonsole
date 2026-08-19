using System;

namespace InfoMasterKonsole
{
    internal static class Program
    {
        private static void Main()
        {
            // Start the application coordinator singleton
            InfoMasterKonsole.Application.Application.Instance.Run();
        }
    }
}
