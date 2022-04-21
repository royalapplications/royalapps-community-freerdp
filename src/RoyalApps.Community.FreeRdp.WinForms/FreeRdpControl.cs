using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using RoyalApps.Community.FreeRdp.WinForms.Configuration;
using RoyalApps.Community.FreeRdp.WinForms.Extensions;

namespace RoyalApps.Community.FreeRdp.WinForms
{
    /// <summary>
    /// Hosts the FreeRDP session created wfreerdp.exe.
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class FreeRdpControl : UserControl
    {
        private const string WFREERDP_EXE = "wfreerdp.exe";

        /// <summary>
        /// 
        /// </summary>
        [Category("FreeRDP Settings"), Description("FreeRDP configuration settings.")]
        public FreeRdpConfiguration Configuration { get; set; } = new();
        
        /// <summary>
        /// Starts the FreeRDP session using the wfreerdp.exe.
        /// </summary>
        public void Connect()
        {
            var arguments = Configuration.GetArguments();

            if (string.IsNullOrEmpty(Configuration.UserName) || string.IsNullOrEmpty(Configuration.Password))
            {
                var credui_info = new CREDUI_INFO
                {
                    hwndParent = Handle,
                    pszCaptionText = $"Authentication Required",
                    pszMessageText = $"Please provide credentials for host: {Configuration.Server}",
                    hbmBanner = IntPtr.Zero,
                };
                credui_info.cbSize = Marshal.SizeOf(credui_info);
                var usernameBuilder = new StringBuilder();
                var passwordBuilder = new StringBuilder();
                var safePassword = false;
                var results = CredentialExtensions.CredUIPromptForCredentialsW(
                    ref credui_info, 
                    Configuration.Server,
                    IntPtr.Zero,
                    0,
                    usernameBuilder,
                    105,
                    passwordBuilder,
                    256,
                    ref safePassword,
                    CREDUI_FLAGS.CREDUIWIN_GENERIC | 
                    CREDUI_FLAGS.CREDUIWIN_ENUMERATE_CURRENT_USER);
                if (results != CredUIReturnCodes.NO_ERROR)
                    throw new InvalidOperationException($"The credential prompt failed: {results}");
                Configuration.UserName = usernameBuilder.ToString();
                Configuration.Password = passwordBuilder.ToString();
            }

            Configuration.ParentWindow = Handle.ToInt64();
            if (Configuration.DesktopWidth == 0 || Configuration.DesktopHeight == 0)
            {
                Configuration.DesktopWidth = ClientSize.Width;
                Configuration.DesktopHeight = ClientSize.Height;
            }

            var freeRdpPath = Environment.ExpandEnvironmentVariables(Path.Combine(Configuration.TempPath, WFREERDP_EXE));
            if (!File.Exists(freeRdpPath))
                File.WriteAllBytes(
                    WFREERDP_EXE,
                    GetType().Assembly.GetResourceFileAsBytes(WFREERDP_EXE));

            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = WFREERDP_EXE;
            process.StartInfo.Arguments = string.Join(" ", arguments);
            process.Start();
        }
    }
}