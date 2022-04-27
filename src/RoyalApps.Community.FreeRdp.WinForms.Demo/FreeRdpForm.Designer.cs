namespace RoyalApps.Community.FreeRdp.WinForms.Demo
{
    partial class FreeRdpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RoyalApps.Community.FreeRdp.WinForms.Configuration.FreeRdpConfiguration freeRdpConfiguration1 = new RoyalApps.Community.FreeRdp.WinForms.Configuration.FreeRdpConfiguration();
            this.FreeRdpControl = new RoyalApps.Community.FreeRdp.WinForms.FreeRdpControl();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisconnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // FreeRdpControl
            // 
            freeRdpConfiguration1.Aero = false;
            freeRdpConfiguration1.AudioCaptureRedirectionMode = false;
            freeRdpConfiguration1.AudioRedirectionMode = RoyalApps.Community.FreeRdp.WinForms.Configuration.AudioRedirectionMode.NotSpecified;
            freeRdpConfiguration1.BitmapCaching = true;
            freeRdpConfiguration1.ColorDepth = RoyalApps.Community.FreeRdp.WinForms.Configuration.BitsPerPixel.NotSpecified;
            freeRdpConfiguration1.Compression = false;
            freeRdpConfiguration1.ConnectToAdministerServer = false;
            freeRdpConfiguration1.DesktopHeight = 0;
            freeRdpConfiguration1.DesktopScaleFactor = 100;
            freeRdpConfiguration1.DesktopWidth = 0;
            freeRdpConfiguration1.DeviceScaleFactor = 100;
            freeRdpConfiguration1.Domain = null;
            freeRdpConfiguration1.GatewayDomain = null;
            freeRdpConfiguration1.GatewayHostname = null;
            freeRdpConfiguration1.GatewayPassword = null;
            freeRdpConfiguration1.GatewayUserName = null;
            freeRdpConfiguration1.GdiRendering = RoyalApps.Community.FreeRdp.WinForms.Configuration.GdiRendering.NotSpecified;
            freeRdpConfiguration1.IgnoreCertificate = true;
            freeRdpConfiguration1.KeyboardLayout = null;
            freeRdpConfiguration1.MenuAnimations = false;
            freeRdpConfiguration1.NetworkConnectionType = RoyalApps.Community.FreeRdp.WinForms.Configuration.NetworkConnectionType.NotSpecified;
            freeRdpConfiguration1.NetworkLevelAuthentication = true;
            freeRdpConfiguration1.ParentWindow = ((long)(0));
            freeRdpConfiguration1.Password = null;
            freeRdpConfiguration1.PCB = null;
            freeRdpConfiguration1.Port = 3389;
            freeRdpConfiguration1.ProtocolSecurityNegotiation = true;
            freeRdpConfiguration1.RedirectClipboard = false;
            freeRdpConfiguration1.Server = null;
            freeRdpConfiguration1.SmoothFonts = false;
            freeRdpConfiguration1.StartProgram = null;
            freeRdpConfiguration1.TempPath = "%temp%";
            freeRdpConfiguration1.Themes = true;
            freeRdpConfiguration1.UserName = null;
            freeRdpConfiguration1.VMId = null;
            freeRdpConfiguration1.Wallpaper = true;
            freeRdpConfiguration1.WindowDrag = false;
            freeRdpConfiguration1.WorkDir = null;
            this.FreeRdpControl.Configuration = freeRdpConfiguration1;
            this.FreeRdpControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FreeRdpControl.Location = new System.Drawing.Point(0, 40);
            this.FreeRdpControl.Name = "FreeRdpControl";
            this.FreeRdpControl.Size = new System.Drawing.Size(2514, 1489);
            this.FreeRdpControl.TabIndex = 0;
            this.FreeRdpControl.Connected += FreeRdpControl_Connected;
            this.FreeRdpControl.Disconnected += FreeRdpControl_Disconnected;
            //
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ConnectionMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(2514, 40);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(71, 36);
            this.FileMenuItem.Text = "&File";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(184, 44);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ConnectionMenuItem
            // 
            this.ConnectionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectMenuItem,
            this.DisconnectMenuItem,
            this.Separator1,
            this.SettingsMenuItem});
            this.ConnectionMenuItem.Name = "ConnectionMenuItem";
            this.ConnectionMenuItem.Size = new System.Drawing.Size(157, 36);
            this.ConnectionMenuItem.Text = "&Connection";
            // 
            // ConnectMenuItem
            // 
            this.ConnectMenuItem.Name = "ConnectMenuItem";
            this.ConnectMenuItem.Size = new System.Drawing.Size(265, 44);
            this.ConnectMenuItem.Text = "C&onnect";
            this.ConnectMenuItem.Click += new System.EventHandler(this.ConnectMenuItem_Click);
            // 
            // DisconnectMenuItem
            // 
            this.DisconnectMenuItem.Enabled = false;
            this.DisconnectMenuItem.Name = "DisconnectMenuItem";
            this.DisconnectMenuItem.Size = new System.Drawing.Size(265, 44);
            this.DisconnectMenuItem.Text = "&Disconnect";
            this.DisconnectMenuItem.Click += new System.EventHandler(this.DisconnectMenuItem_Click);
            // 
            // Separator1
            // 
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(262, 6);
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(265, 44);
            this.SettingsMenuItem.Text = "&Settings...";
            this.SettingsMenuItem.Click += new System.EventHandler(this.SettingsMenuItem_Click);
            // 
            // FreeRdpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2514, 1529);
            this.Controls.Add(this.FreeRdpControl);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "FreeRdpForm";
            this.Text = "FreeRDP Test";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FreeRdpControl FreeRdpControl;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisconnectMenuItem;
        private System.Windows.Forms.ToolStripSeparator Separator1;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenuItem;
    }
}