﻿using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;

namespace RoyalApps.Community.FreeRdp.WinForms.Demo;

public partial class FreeRdpForm : Form
{

    private readonly Form _form;
    private readonly PropertyGrid _propertyGrid;
    
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
            using var inputDialog = new InputDialog
            {
                WindowTitle = @"Server Required",
                MainInstruction = @"Please enter a server name or IP address",
                Content = @"Note: The FreeRdpControl will throw an exception if the Server property is not specified.",
                Input = FreeRdpControl.Configuration.Server
            };
            
            if (inputDialog.ShowDialog(this) == DialogResult.Cancel ||
                string.IsNullOrWhiteSpace(inputDialog.Input))
                return;

            FreeRdpControl.Configuration.Server = inputDialog.Input;
        }

        if (string.IsNullOrEmpty(FreeRdpControl.Configuration.UserName) || 
            string.IsNullOrEmpty(FreeRdpControl.Configuration.Password))
        {
            var credentials = GetCredentialFromDialog(@"Please enter a username and password");
            FreeRdpControl.Configuration.UserName = credentials?.UserName;
            FreeRdpControl.Configuration.Domain = string.IsNullOrWhiteSpace(credentials?.Domain) 
                ? null 
                : credentials.Domain;
            FreeRdpControl.Configuration.Password = credentials?.Password;
        }
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
        e.SetCredentials(credentials.UserName, credentials.Domain, credentials.Password);
    }

    private NetworkCredential? GetCredentialFromDialog(string mainInstruction)
    {
        using var credentialDialog = new CredentialDialog
        {
            Target = FreeRdpControl.Configuration.Server,
            WindowTitle = @"Credentials Required",
            MainInstruction = mainInstruction,
            Content = @"Note: The FreeRdpControl will throws an exception if no credentials are provided.",
            ShowSaveCheckBox = false,
        };
        credentialDialog.ShowDialog(this);
        return credentialDialog.Credentials;
    }
}