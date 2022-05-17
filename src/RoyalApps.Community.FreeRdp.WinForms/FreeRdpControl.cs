using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using RoyalApps.Community.FreeRdp.WinForms.Configuration;
using RoyalApps.Community.FreeRdp.WinForms.Extensions;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace RoyalApps.Community.FreeRdp.WinForms
{
    /// <summary>
    /// Hosts the FreeRDP session created wfreerdp.exe.
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class FreeRdpControl : UserControl
    {
        private const string WFREERDP_EXE = "wfreerdp.exe";

        private readonly Timer _timerResizeInProgress;
        private readonly UserControl _renderTarget;
        private Size _previousClientSize = Size.Empty;
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
        /// FreeRdpControl constructor
        /// </summary>
        public FreeRdpControl()
        {
            _renderTarget = new UserControl
            {
                Anchor = AnchorStyles.None,
                Dock = DockStyle.None,
            };
            
            _timerResizeInProgress = new Timer
            {
                Interval = 1000
            };
            _timerResizeInProgress.Tick += TimerResizeInProgress_Tick;
        }

        /// <summary>
        /// Clean up resources
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timerResizeInProgress.Tick -= TimerResizeInProgress_Tick;
                _timerResizeInProgress.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnLoad override
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            AutoScroll = true;

            _renderTarget.Parent = this;
        }

        /// <summary>
        /// OnSizeChanged override
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!Configuration.SmartReconnect)
                return;
            _timerResizeInProgress.Start();
        }

        /// <summary>
        /// Starts the FreeRDP session using the wfreerdp.exe
        /// </summary>
        public void Connect()
        {
            if (_process is {HasExited: false})
                return;
            
            var arguments = Configuration.GetArguments();
            Configuration.ParentWindow = _renderTarget.Handle.ToInt64();
            if (Configuration.DesktopWidth == 0 || Configuration.DesktopHeight == 0)
            {
                Configuration.DesktopWidth = ClientSize.Width;
                Configuration.DesktopHeight = ClientSize.Height;
            }

            // calculate the size of the render target based on the remote desktop size
            _renderTarget.MinimumSize = Size.Empty;
            _renderTarget.MaximumSize = Size.Empty;
            _renderTarget.Size = new Size(Configuration.DesktopWidth, Configuration.DesktopHeight);
            _renderTarget.MinimumSize = _renderTarget.Size;
            _renderTarget.MaximumSize = _renderTarget.Size;
            // calculate position, since anchor and dock is none, it will be kept in center
            _renderTarget.Location = new Point(
                ClientSize.Width / 2 - _renderTarget.Width / 2, 
                ClientSize.Height / 2 - _renderTarget.Height / 2);

            _previousClientSize = ClientSize;

            // AutoScrollMinSize is required to get scrollbars to appear
            AutoScrollMinSize = _renderTarget.Size;
            
            var freeRdpPath = Environment.ExpandEnvironmentVariables(Path.Combine(Configuration.TempPath, WFREERDP_EXE));
            if (!File.Exists(freeRdpPath))
                File.WriteAllBytes(
                    freeRdpPath,
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

        /// <summary>
        /// Ends the FreeRDP session by ending the wfreerdp.exe process.
        /// </summary>
        public void Disconnect()
        {
            KillProcess();            
            Invoke(Disconnected, this, new DisconnectEventArgs(0) { UserInitiated = true });
        }
        
        private void Process_Exited(object? sender, EventArgs e)
        {
            if (_process is null)
                return;
            
            var exitCode = _process.ExitCode;
            _process.Exited -= Process_Exited;
            _process.Dispose();
            _process = null;

            Invoke(Disconnected, this, new DisconnectEventArgs((uint)exitCode));
        }
        
        private void TimerResizeInProgress_Tick(object? sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
                return;
            _timerResizeInProgress.Stop();
            
            // make sure that Size 0,0 (when minimized) is also ignored
            if (Size.Width == 0 ||
                Size.Height == 0 ||
                _previousClientSize.IsEmpty ||
                _previousClientSize.Equals(Size))
                return;

            KillProcess();
            
            Configuration.DesktopWidth = 0;
            Configuration.DesktopHeight = 0;
            
            Connect();
        }

        private void KillProcess()
        {
            if (_process is null || _process.HasExited)
                return;
            
            _process.Exited -= Process_Exited;
            _process.Kill();
            _process = null;
        }
        
   }
}