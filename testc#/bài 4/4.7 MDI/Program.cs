namespace Bai4_7
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            while (true)
            {
                using var login = new FormLogin();
                if (login.ShowDialog() != DialogResult.OK) break;

                var main = new FormMain(login.UserName);
                Application.Run(main);
                if (!main.LoggedOut) break;  
            }
        }
    }
}
