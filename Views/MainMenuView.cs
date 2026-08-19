namespace InfoMasterKonsole.Views;

public class MainMenuView
{
    public void Show()
    {
        Console.Clear();
        Console.WriteLine("==========================================");
        Console.WriteLine("       CUSTOMER DATA EXCHANGE SYSTEM");
        Console.WriteLine("==========================================");
        Console.WriteLine("1. Customer Management");
        Console.WriteLine("2. Import / Export");
        Console.WriteLine("3. Search Customers");
        Console.WriteLine("4. Customer Summary");
        Console.WriteLine("5. Exit");
        Console.WriteLine("==========================================");
    }

    public string ReadChoice()
    {
        Console.Write("Enter choice: ");
        return Console.ReadLine();
    }
}
