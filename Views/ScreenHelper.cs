using System;

namespace InfoMasterKonsole.Views
{
    /// <summary>
    /// Small helper for console screen management. Keeps screen-clearing logic centralized for Views.
    /// </summary>
    public static class ScreenHelper
    {
        public static void ShowTitle(string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(new string('=', Math.Max(10, title.Length)));
            Console.WriteLine();
        }

        public static void PauseAndClear()
        {
            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
