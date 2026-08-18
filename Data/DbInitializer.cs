using System;
using System.IO;

namespace InfoMasterKonsole.Data
{
    /// <summary>
    /// Simple database initializer. Consumers can call Initialize() at application startup
    /// to ensure the SQLite database file and schema exist. Uses EnsureCreated() for simplicity.
    /// </summary>
    public class DbInitializer
    {
        public void Initialize()
        {
            // Ensure data folder exists; the DbContext will create the database file when needed.
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataDir = Path.Combine(baseDir, "data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            // Create the database schema if it does not exist. Keep EF Core calls inside a try/catch when used.
            try
            {
                using (var context = new CustomerDbContext())
                {
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {
                // Swallow initialization exceptions at this stage; callers should handle/log as needed.
            }
        }
    }
}
