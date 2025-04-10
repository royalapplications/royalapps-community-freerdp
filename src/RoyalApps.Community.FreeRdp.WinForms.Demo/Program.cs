using System;
using System.Windows.Forms;

namespace RoyalApps.Community.FreeRdp.WinForms.Demo;

public static class Program
{
    [STAThread]
    private static void Main(params string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.Run(new FreeRdpForm());
    }
}