namespace HospitalMdi;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        while (true)
        {
            using var login = new frmLogin();
            if (login.ShowDialog() != DialogResult.OK) break;
            var main = new frmHospitalMain(login.DutyName);
            Application.Run(main);
            if (!main.LoggedOut) break;
        }
    }
}
