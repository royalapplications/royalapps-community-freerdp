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
            this.UseCredManMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ResetZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomInMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // FreeRdpControl
            // 
            this.FreeRdpControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FreeRdpControl.Location = new System.Drawing.Point(0, 40);
            this.FreeRdpControl.Name = "FreeRdpControl";
            this.FreeRdpControl.Size = new System.Drawing.Size(2514, 1489);
            this.FreeRdpControl.TabIndex = 0;
            this.FreeRdpControl.Connected += FreeRdpControl_Connected;
            this.FreeRdpControl.Disconnected += FreeRdpControl_Disconnected;
            this.FreeRdpControl.CertificateError += FreeRdpControl_CertificateError;
            this.FreeRdpControl.VerifyCredentials += FreeRdpControl_VerifyCredentials; 
            //
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ConnectionMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(2514, 42);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(71, 38);
            this.FileMenuItem.Text = "&File";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(359, 44);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ConnectionMenuItem
            // 
            this.ConnectionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectMenuItem,
            this.DisconnectMenuItem,
            this.Separator2,
            this.ZoomInMenuItem,
            this.ZoomOutMenuItem,
            this.ResetZoomMenuItem,
            this.Separator1,
            this.SettingsMenuItem,
            this.UseCredManMenuItem
            });
            this.ConnectionMenuItem.Name = "ConnectionMenuItem";
            this.ConnectionMenuItem.Size = new System.Drawing.Size(157, 38);
            this.ConnectionMenuItem.Text = "&Connection";
            // 
            // ConnectMenuItem
            // 
            this.ConnectMenuItem.Name = "ConnectMenuItem";
            this.ConnectMenuItem.Size = new System.Drawing.Size(359, 44);
            this.ConnectMenuItem.Text = "C&onnect";
            this.ConnectMenuItem.Click += new System.EventHandler(this.ConnectMenuItem_Click);
            // 
            // DisconnectMenuItem
            // 
            this.DisconnectMenuItem.Enabled = false;
            this.DisconnectMenuItem.Name = "DisconnectMenuItem";
            this.DisconnectMenuItem.Size = new System.Drawing.Size(359, 44);
            this.DisconnectMenuItem.Text = "&Disconnect";
            this.DisconnectMenuItem.Click += new System.EventHandler(this.DisconnectMenuItem_Click);
            // 
            // Separator1
            // 
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(356, 6);
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(359, 44);
            this.SettingsMenuItem.Text = "&Settings...";
            this.SettingsMenuItem.Click += new System.EventHandler(this.SettingsMenuItem_Click);
            //
            // UseCredManMenuItem
            //
            this.UseCredManMenuItem.Name = "UseCredManMenuItem";
            this.UseCredManMenuItem.Size = new System.Drawing.Size(359, 44);
            this.UseCredManMenuItem.Text = "&Use Credential Manager";
            this.UseCredManMenuItem.Click += new System.EventHandler(this.UseCredManMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.Separator2.Name = "Separator2";
            this.Separator2.Size = new System.Drawing.Size(356, 6);
            // 
            // ResetZoomMenuItem
            // 
            this.ResetZoomMenuItem.Name = "ResetZoomMenuItem";
            this.ResetZoomMenuItem.Size = new System.Drawing.Size(359, 44);
            this.ResetZoomMenuItem.Text = "&Reset Zoom";
            this.ResetZoomMenuItem.Click += new System.EventHandler(this.ResetZoomMenuItem_Click);
            // 
            // ZoomInMenuItem
            // 
            this.ZoomInMenuItem.Name = "ZoomInMenuItem";
            this.ZoomInMenuItem.Size = new System.Drawing.Size(359, 44);
            this.ZoomInMenuItem.Text = "Zoom &In";
            this.ZoomInMenuItem.Click += new System.EventHandler(this.ZoomInMenuItem_Click);
            // 
            // ZoomOutMenuItem
            // 
            this.ZoomOutMenuItem.Name = "ZoomOutMenuItem";
            this.ZoomOutMenuItem.Size = new System.Drawing.Size(359, 44);
            this.ZoomOutMenuItem.Text = "Zoom O&ut";
            this.ZoomOutMenuItem.Click += new System.EventHandler(this.ZoomOutMenuItem_Click);
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
        private System.Windows.Forms.ToolStripSeparator Separator2;
        private System.Windows.Forms.ToolStripMenuItem ZoomInMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ZoomOutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResetZoomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UseCredManMenuItem;
    }
}