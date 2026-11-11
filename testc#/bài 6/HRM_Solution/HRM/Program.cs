using System;
using System.Windows.Forms;
using HRM.GUI;

namespace HRM
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain());
        }
    }
}
