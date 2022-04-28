using System;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;

namespace RoyalApps.Community.FreeRdp.WinForms.Demo;

public partial class FreeRdpForm : Form
{
    public FreeRdpForm()
    {
        InitializeComponent();
    }

    private void ExitMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ConnectMenuItem_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FreeRdpControl.Configuration.Server))
        {
            using var inputDialog = new InputDialog
            {
                WindowTitle = @"Server Required",
                MainInstruction = @"Please enter a server name or IP address",
                Content = @"Note: The FreeRdpControl will throw an exception if the Server property is not specified."
            };
            
            if (inputDialog.ShowDialog(this) == DialogResult.Cancel ||
                string.IsNullOrWhiteSpace(inputDialog.Input))
                return;

            FreeRdpControl.Configuration.Server = inputDialog.Input;
        }

        if (string.IsNullOrEmpty(FreeRdpControl.Configuration.UserName) || 
            string.IsNullOrEmpty(FreeRdpControl.Configuration.Password))
        {
            using var credentialDialog = new CredentialDialog
            {
                Target = FreeRdpControl.Configuration.Server,
                WindowTitle = @"Credentials Required",
                MainInstruction = @"Please enter a username and password",
                Content = @"Note: The FreeRdpControl will throws an exception if no credentials are provided.",
                ShowSaveCheckBox = false,
            };
            if (credentialDialog.ShowDialog(this) == DialogResult.Cancel ||
                credentialDialog.Credentials == null)
                return;
            
            FreeRdpControl.Configuration.UserName = credentialDialog.Credentials.UserName;
            FreeRdpControl.Configuration.Domain = credentialDialog.Credentials.Domain;
            FreeRdpControl.Configuration.Password = credentialDialog.Password;
        }
        FreeRdpControl.Connect();
    }

    private void DisconnectMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void SettingsMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void FreeRdpControl_Connected(object sender, EventArgs e)
    {
        ConnectMenuItem.Enabled = false;
        DisconnectMenuItem.Enabled = true;
    }

    private void FreeRdpControl_Disconnected(object sender, DisconnectEventArgs e)
    {
        ConnectMenuItem.Enabled = false;
        DisconnectMenuItem.Enabled = true;
        MessageBox.Show(this, @$"Exit Code: {e.ExitCode}", @"Process wfreerdp.exe was stopped");
    }
}