using System;
using System.IO;

namespace InfoMasterKonsole.Exceptions
{
    /// <summary>
    /// Minimal exception logger that writes exception details to a log file under ./data/logs.txt.
    /// It uses a simple static method to keep usage straightforward across the app.
    /// </summary>
    public static class ExceptionLogger
    {
        private static readonly object _sync = new object();

        public static void Log(Exception ex)
        {
            try
            {
                var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
                var dataDir = Path.Combine(baseDir, "data", "logs");
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }

                var logPath = Path.Combine(dataDir, "errors.log");
                var text = $"[{DateTime.UtcNow:O}] {ex}\n";

                lock (_sync)
                {
                    File.AppendAllText(logPath, text);
                }
            }
            catch
            {
                // If logging fails, avoid throwing further exceptions.
            }
        }
    }
}
