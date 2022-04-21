using System;
using System.Windows.Forms;

using Microsoft.VisualBasic;

namespace RoyalApps.Community.FreeRdp.WinForms.Demo;

public partial class FreeRdpForm : Form
{
    public FreeRdpForm()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (string.IsNullOrWhiteSpace(freeRdpControl1.Configuration.Server))
        {
            freeRdpControl1.Configuration.Server = Interaction.InputBox("Please enter a server name or IP address:", "Server Required");
        }

        freeRdpControl1.Connect();
    }
}