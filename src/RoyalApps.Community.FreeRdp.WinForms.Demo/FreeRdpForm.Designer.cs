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
            this.freeRdpControl1 = new RoyalApps.Community.FreeRdp.WinForms.FreeRdpControl();
            this.SuspendLayout();
            // 
            // freeRdpControl1
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
            freeRdpConfiguration1.PCB = null;
            freeRdpConfiguration1.Port = 3389;
            freeRdpConfiguration1.ProtocolSecurityNegotiation = true;
            freeRdpConfiguration1.RedirectClipboard = false;
            freeRdpConfiguration1.SmoothFonts = false;
            freeRdpConfiguration1.StartProgram = null;
            freeRdpConfiguration1.TempPath = "%temp%";
            freeRdpConfiguration1.Themes = true;
            freeRdpConfiguration1.VMId = null;
            freeRdpConfiguration1.Wallpaper = true;
            freeRdpConfiguration1.WindowDrag = false;
            freeRdpConfiguration1.WorkDir = null;
            this.freeRdpControl1.Configuration = freeRdpConfiguration1;
            this.freeRdpControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.freeRdpControl1.Location = new System.Drawing.Point(0, 0);
            this.freeRdpControl1.Name = "freeRdpControl1";
            this.freeRdpControl1.Size = new System.Drawing.Size(998, 697);
            this.freeRdpControl1.TabIndex = 0;
            // 
            // FreeRdpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(998, 697);
            this.Controls.Add(this.freeRdpControl1);
            this.Name = "FreeRdpForm";
            this.Text = "FreeRDP Test";
            this.ResumeLayout(false);

        }

        #endregion

        private FreeRdpControl freeRdpControl1;
    }
}