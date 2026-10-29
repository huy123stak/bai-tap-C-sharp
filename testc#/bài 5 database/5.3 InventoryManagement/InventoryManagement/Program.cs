using InventoryManagement.GUI;

namespace InventoryManagement;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}
