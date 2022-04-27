using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
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
        private Process? _process;

        /// <summary>
        /// FreeRDP configuration settings 
        /// </summary>
        [Category("FreeRDP Settings"), Description("FreeRDP configuration settings.")]
        public FreeRdpConfiguration Configuration { get; set; } = new();

        /// <summary>
        /// Raised when wfreerdp.exe has been started.
        /// </summary>
        public event EventHandler? Connected;

        /// <summary>
        /// Raised when wfreerdp.exe has exited.
        /// </summary>
        public event EventHandler<DisconnectEventArgs>? Disconnected; 

        /// <summary>
        /// Starts the FreeRDP session using the wfreerdp.exe
        /// </summary>
        public void Connect()
        {
            if (_process is {HasExited: false})
                return;
            
            var arguments = Configuration.GetArguments();

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

            _process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = freeRdpPath,
                    Arguments = string.Join(" ", arguments)
                }
            };
            _process.Exited += Process_Exited;
            _process.Start();
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            if (_process is null)
                return;
            
            var exitCode = _process.ExitCode;
            _process.Exited -= Process_Exited;
            _process.Dispose();
            _process = null;

            Invoke(Disconnected, this, new DisconnectEventArgs(exitCode));
        }
    }
}