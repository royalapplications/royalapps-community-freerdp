using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;
using RoyalApps.Community.FreeRdp.WinForms.Configuration;
using Simple.CredentialManager;

namespace RoyalApps.Community.FreeRdp.WinForms.Demo;

public partial class FreeRdpForm : Form
{
    private const string TargetPrefix = "TERMSRV/";

    private readonly Form _form;
    private readonly PropertyGrid _propertyGrid;
    private Credential? _credential;
    private Credential? _credentialGateway;
    private bool _credentialExisted;
    private bool _credentialGatewayExisted;
    
    public FreeRdpForm()
    {
        InitializeComponent();
        
        _form = new Form
        {
            Size = new Size(800, 1000),
            Text = @"Settings",
            FormBorderStyle = FormBorderStyle.SizableToolWindow,
            StartPosition = FormStartPosition.CenterParent
        };
        _form.Closing += (_, args) =>
        {
            _form.Hide();
            args.Cancel = true;
        };

        _propertyGrid = new PropertyGrid
        {
            Dock = DockStyle.Fill
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _form.Size = new Size(800, 1000);
        _propertyGrid.Parent = _form;
    }
    
    private void ExitMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ConnectMenuItem_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FreeRdpControl.Configuration.Server))
        {
            using var inputDialog = new InputDialog();
            inputDialog.WindowTitle = @"Server Required";
            inputDialog.MainInstruction = @"Please enter a server name or IP address";
            inputDialog.Content = @"Note: The FreeRdpControl will throw an exception if the Server property is not specified.";
            inputDialog.Input = FreeRdpControl.Configuration.Server;

            if (inputDialog.ShowDialog(this) == DialogResult.Cancel ||
                string.IsNullOrWhiteSpace(inputDialog.Input))
                return;

            FreeRdpControl.Configuration.Server = inputDialog.Input;
        }

        if (string.IsNullOrEmpty(FreeRdpControl.Configuration.Username) || 
            string.IsNullOrEmpty(FreeRdpControl.Configuration.Password))
        {
            var credentials = GetCredentialFromDialog(@"Please enter a username and password");
            FreeRdpControl.Configuration.Username = credentials?.UserName;
            FreeRdpControl.Configuration.Domain = string.IsNullOrWhiteSpace(credentials?.Domain) 
                ? null 
                : credentials.Domain;
            FreeRdpControl.Configuration.Password = credentials?.Password;
        }

        _credential = HandleMainCredentials(FreeRdpControl.Configuration, out _credentialExisted);
        _credentialGateway = HandleGatewayCredentials(FreeRdpControl.Configuration, out _credentialGatewayExisted);
        
        FreeRdpControl.Connect();
    }

    private void DisconnectMenuItem_Click(object sender, EventArgs e)
    {
        FreeRdpControl.Disconnect();
    }

    private void ZoomInMenuItem_Click(object sender, EventArgs e)
    {
        FreeRdpControl.ZoomIn();
    }

    private void ZoomOutMenuItem_Click(object sender, EventArgs e)
    {
        FreeRdpControl.ZoomOut();
    }

    private void ResetZoomMenuItem_Click(object sender, EventArgs e)
    {
        FreeRdpControl.ResetZoom();
    }

    private void SettingsMenuItem_Click(object sender, EventArgs e)
    {
        _propertyGrid.SelectedObject = FreeRdpControl.Configuration;
        _form.Show(this);
    }

    private void UseCredManMenuItem_Click(object? sender, EventArgs e)
    {
        UseCredManMenuItem.Checked = !UseCredManMenuItem.Checked;
    }

    private void FreeRdpControl_Connected(object sender, EventArgs e)
    {
        ConnectMenuItem.Enabled = false;
        DisconnectMenuItem.Enabled = true;
        _propertyGrid.SelectedObject = FreeRdpControl.Configuration;
    }

    private void FreeRdpControl_Disconnected(object sender, DisconnectEventArgs e)
    {
        ConnectMenuItem.Enabled = true;
        DisconnectMenuItem.Enabled = false;
        _propertyGrid.SelectedObject = FreeRdpControl.Configuration;

        if (!_credentialExisted)
            _credential?.Delete();
        _credential?.Dispose();
        _credential = null;

        if (!_credentialGatewayExisted)
            _credentialGateway?.Delete();
        _credentialGateway?.Dispose();
        _credentialGateway = null;
        
        if (e.UserInitiated)
            return;
        MessageBox.Show(this, e.ErrorMessage, @"RDP Session Terminated", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void FreeRdpControl_CertificateError(object sender, CertificateErrorEventArgs e)
    {
        if (MessageBox.Show(
                this, 
                @"The hostname of the server certificate does not match the provided host name. Do you want to ignore this error and try to connect again?", 
                @"FreeRdp TLS handshake Error",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) 
            e.Continue();
    }

    private void FreeRdpControl_VerifyCredentials(object? sender, VerifyCredentialsEventArgs e)
    {
        var credentials = GetCredentialFromDialog(@"An authentication error occurred. Login failed. Please verify your credentials.");
        if (credentials == null)
            return;

        if (UseCredManMenuItem.Checked)
        {
            FreeRdpControl.Configuration.Username = credentials.UserName;
            FreeRdpControl.Configuration.Domain = credentials.Domain;
            FreeRdpControl.Configuration.Password = credentials.Password;
            _credential = HandleMainCredentials(FreeRdpControl.Configuration, out _);
            e.SetCredentials(null, null, null);
        }
        else
        {
            e.SetCredentials(credentials.UserName, credentials.Domain, credentials.Password);
        }
    }

    private NetworkCredential? GetCredentialFromDialog(string mainInstruction)
    {
        using var credentialDialog = new CredentialDialog();
        credentialDialog.Target = FreeRdpControl.Configuration.Server;
        credentialDialog.WindowTitle = @"Credentials Required";
        credentialDialog.MainInstruction = mainInstruction;
        credentialDialog.Content = @"Note: The FreeRdpControl will throws an exception if no credentials are provided.";
        credentialDialog.ShowSaveCheckBox = false;
        credentialDialog.ShowDialog(this);
        return credentialDialog.Credentials;
    }

    private Credential? HandleMainCredentials(FreeRdpConfiguration configuration, out bool credentialExisted)
    {
        credentialExisted = false;
        
        if (!UseCredManMenuItem.Checked)
            return null;
        
        var credential = new Credential {Target = $"{TargetPrefix}{configuration.Server}"};
        credentialExisted = credential.Load();
        credential.Username = string.IsNullOrWhiteSpace(configuration.Domain) 
            ? configuration.Username 
            : $"{configuration.Domain}\\{configuration.Username}";
        credential.Password = configuration.Password;
        credential.Save();

        configuration.Username = null;
        configuration.Domain = null;
        configuration.Password = null;
        return credential;
    }
    
    private Credential? HandleGatewayCredentials(FreeRdpConfiguration configuration, out bool credentialExisted)
    {
        credentialExisted = false;
        
        if (!UseCredManMenuItem.Checked)
            return null;

        if (string.IsNullOrWhiteSpace(configuration.Gateway.Hostname) &&
            string.IsNullOrWhiteSpace(configuration.Gateway.Username) &&
            string.IsNullOrWhiteSpace(configuration.Gateway.Password))
        {
            return null;
        }

        var credential = new Credential {Target = $"{TargetPrefix}{configuration.Gateway.Hostname}"};
        credentialExisted = credential.Load();
        credential.Username = string.IsNullOrWhiteSpace(configuration.Gateway.Domain) 
            ? configuration.Gateway.Username 
            : $"{configuration.Gateway.Domain}\\{configuration.Gateway.Username}";
        credential.Password = configuration.Gateway.Password;
        credential.Save();

        configuration.Gateway.Username = null;
        configuration.Gateway.Domain = null;
        configuration.Gateway.Password = null;
        return credential;
    }
}