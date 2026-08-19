namespace InfoMasterKonsole.Views;

public class ImportExportView
{
    public void Show()
    {
        Console.Clear();
        Console.WriteLine("==========================================");
        Console.WriteLine("             IMPORT / EXPORT");
        Console.WriteLine("==========================================");
        Console.WriteLine("1. Export JSON");
        Console.WriteLine("2. Export XML");
        Console.WriteLine("3. Import JSON");
        Console.WriteLine("4. Import XML");
        Console.WriteLine("5. Back");
        Console.WriteLine("==========================================");
        Console.Write("Enter choice: ");
    }
}
