using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Windows.Win32;
using Microsoft.Extensions.Logging;
using RoyalApps.Community.FreeRdp.WinForms.Configuration;
using RoyalApps.Community.FreeRdp.WinForms.Extensions;
using RoyalApps.Community.FreeRdp.WinForms.Logging;

namespace RoyalApps.Community.FreeRdp.WinForms;

/// <summary>
/// Hosts the FreeRDP session created wfreerdp.exe.
/// </summary>
[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
public class FreeRdpControl : UserControl
{
    private static bool _executableWritten;

    private static readonly ProcessJobTracker ProcessJobTracker = new("royalapps_wfreerdp");

    private const string WFREERDP_EXE = "wfreerdp.exe";
    private const int PROCESS_EXIT_TIMEOUT_MILLISECONDS = 2000;

    private readonly object _processSyncRoot = new();
    private readonly Timer _timerResizeInProgress;
    private readonly UserControl _renderTarget;
    private Size _previousClientSize = Size.Empty;
    private Process? _process;
    private long _processGeneration;
    private IntPtr _freeRdpWindowHandle = IntPtr.Zero;
    private int _disposeStarted;

    private int _initialZoomFactor = 100;
    private int _currentZoomFactor = 100;
    private int _initialDesktopWidth = -1;
    private int _initialDesktopHeight = -1;

    /// <summary>
    /// FreeRDP configuration settings
    /// </summary>
    [Category("FreeRDP Settings"), Description("FreeRDP configuration settings.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FreeRdpConfiguration Configuration { get; set; } = new();

    /// <summary>
    /// Logger instance
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ILogger Logger { get; set; } = DebugLoggerFactory.Create();

    /// <summary>
    /// Raised when wfreerdp.exe has been started.
    /// </summary>
    public event EventHandler? Connected;

    /// <summary>
    /// Raised when wfreerdp.exe has exited.
    /// </summary>
    public event EventHandler<DisconnectEventArgs>? Disconnected;

    /// <summary>
    /// Raised when the TLS handshake fails because if an incorrect server certificate.
    /// </summary>
    public event EventHandler<CertificateErrorEventArgs>? CertificateError;

    /// <summary>
    /// Raised when login failed.
    /// </summary>
    public event EventHandler<VerifyCredentialsEventArgs>? VerifyCredentials;

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
        if (disposing && System.Threading.Interlocked.Exchange(ref _disposeStarted, 1) == 0)
        {
            _timerResizeInProgress.Stop();
            _timerResizeInProgress.Tick -= TimerResizeInProgress_Tick;
            _timerResizeInProgress.Dispose();
            StopProcess();
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc cref="WndProc"/>
    protected override void WndProc(ref Message m)
    {
        var message = (uint)m.Msg;
        var isFocusMessage = message is PInvoke.WM_MOUSEACTIVATE or PInvoke.WM_SETFOCUS;

        if (isFocusMessage && !CanProcessFocusMessage())
            return;

        base.WndProc(ref m);

        if (!isFocusMessage || !CanForwardFocus())
            return;

        switch (message)
        {
            case PInvoke.WM_MOUSEACTIVATE:
                if (!_renderTarget.Focused)
                {
                    _renderTarget.Focus();
                    if (!CanForwardFocus())
                        return;

                    SetFocusToFreeRdpWindow();
                }
                break;
            case PInvoke.WM_SETFOCUS:
                SetFocusToFreeRdpWindow();
                break;
        }
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
        if (DisposeStarted || Disposing || IsDisposed || !Configuration.SmartReconnect)
            return;
        _timerResizeInProgress.Start();
    }

    /// <summary>
    /// Starts the FreeRDP session using the wfreerdp.exe
    /// </summary>
    public void Connect()
    {
        ObjectDisposedException.ThrowIf(DisposeStarted, this);

        Process? previousProcess;
        long connectGeneration;
        lock (_processSyncRoot)
        {
            if (_process is {HasExited: false})
                return;

            previousProcess = _process;
            _process = null;
            connectGeneration = unchecked(++_processGeneration);
        }

        if (previousProcess is not null)
        {
            previousProcess.Exited -= Process_Exited;
            previousProcess.Dispose();
        }

        if (DisposeStarted)
            throw new ObjectDisposedException(nameof(FreeRdpControl));

        _freeRdpWindowHandle = IntPtr.Zero;

        ApplyAutoScaling();

        if (Configuration.DesktopWidth == 0 || Configuration.DesktopHeight == 0)
        {
            Configuration.DesktopWidth = ClientSize.Width;
            Configuration.DesktopHeight = ClientSize.Height;
        }

        Configuration.ParentWindow = _renderTarget.Handle.ToInt64();

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
        if (!string.IsNullOrWhiteSpace(Configuration.Executable))
        {
            var customPath = Environment.ExpandEnvironmentVariables(Configuration.Executable!);
            if (File.Exists(customPath))
            {
                freeRdpPath = customPath;
            }
        }
        else
        {
            VerifyExecutable(freeRdpPath);
        }

        var arguments = Configuration.GetArguments().Where(a => a.Any());
        var process = new Process
        {
            EnableRaisingEvents = true,
            StartInfo =
            {
                UseShellExecute = false,
                FileName = freeRdpPath,
                Arguments = string.Join(" ", arguments).Trim(),
                WorkingDirectory = Environment.ExpandEnvironmentVariables(Configuration.TempPath)
            }
        };

        Logger.LogTrace("Starting wfreerdp.exe {Arguments}", process.StartInfo.Arguments);

        var processOwned = false;
        var connectSuperseded = false;
        process.Exited += Process_Exited;
        try
        {
            lock (_processSyncRoot)
            {
                if (DisposeStarted)
                    throw new ObjectDisposedException(nameof(FreeRdpControl));

                if (_processGeneration != connectGeneration || _process is not null)
                {
                    connectSuperseded = true;
                }
                else
                {
                    _process = process;
                    processOwned = true;
                    process.Start();
                    ProcessJobTracker.AddProcess(process);
                }
            }
        }
        catch
        {
            if (processOwned)
            {
                StopProcess(process);
            }
            else
            {
                process.Exited -= Process_Exited;
                process.Dispose();
            }
            throw;
        }

        if (connectSuperseded)
        {
            process.Exited -= Process_Exited;
            process.Dispose();
            return;
        }

        OnConnected(connectGeneration);
    }

    /// <summary>
    /// Ends the FreeRDP session by ending the wfreerdp.exe process.
    /// </summary>
    public void Disconnect()
    {
        StopProcess();
        OnDisconnected(new DisconnectEventArgs(0) {UserInitiated = true});
    }

    /// <summary>
    /// Resets the zoom factor to the initial zoom factor
    /// </summary>
    public void ResetZoom()
    {
        SetZoomLevel(_initialZoomFactor);
    }

    /// <summary>
    /// Sets the desired zoom level (DPI) in percent of the remote desktop session.
    /// The value must be in the range between 100 and 500.
    /// </summary>
    /// <param name="scalingInPercent">Scaling factor in percent</param>
    public void SetZoomLevel(int scalingInPercent)
    {
        Configuration.AutoScaling = false;
        Configuration.DesktopScaleFactor = scalingInPercent switch
        {
            <= 100 => 100,
            >= 500 => 500,
            _ => scalingInPercent
        };
        Configuration.DeviceScaleFactor = Configuration.DesktopScaleFactor switch
        {
            <= 100 => 100,
            < 200 => 140,
            >= 200 => 180,
        };
        _currentZoomFactor = scalingInPercent;

        Reconnect();
    }

    /// <summary>
    /// Increase zoom factor
    /// </summary>
    public void ZoomIn()
    {
        var newScaleFactor = _currentZoomFactor switch
        {
            100 => 125,
            125 => 150,
            150 => 175,
            175 => 200,
            200 => 225,
            225 => 250,
            250 => 300,
            300 => 350,
            350 => 400,
            400 => 450,
            450 => 500,
            _ => 100
        };
        SetZoomLevel(newScaleFactor);
    }

    /// <summary>
    /// Decrease zoom factor
    /// </summary>
    public void ZoomOut()
    {
        var newScaleFactor = _currentZoomFactor switch
        {
            500 => 450,
            450 => 400,
            400 => 350,
            350 => 300,
            300 => 250,
            250 => 225,
            225 => 200,
            200 => 175,
            175 => 150,
            150 => 125,
            _ => 100
        };
        SetZoomLevel(newScaleFactor);
    }

    private void Process_Exited(object? sender, EventArgs e)
    {
        if (sender is not Process process)
            return;

        long processGeneration;
        lock (_processSyncRoot)
        {
            if (!ReferenceEquals(_process, process))
                return;

            processGeneration = _processGeneration;
            _process = null;
        }

        var exitCode = process.ExitCode;
        process.Exited -= Process_Exited;
        process.Dispose();

        if (!CanRaiseLifecycleEvent)
            return;

        InvokeIfLifecycleActive(() => HandleProcessExit(processGeneration, exitCode));
    }

    private void HandleProcessExit(long processGeneration, int exitCode)
    {
        if (!IsProcessGenerationCurrent(processGeneration))
            return;

        _freeRdpWindowHandle = IntPtr.Zero;

        // invalid cert
        if (exitCode == 131080 && !Configuration.Certificate.Ignore)
        {
            var args = new CertificateErrorEventArgs();
            OnCertificateError(args);

            if (!IsProcessGenerationCurrent(processGeneration))
                return;

            if (args.ShouldContinue)
            {
                Configuration.Certificate.Ignore = true;
                Reconnect(processGeneration);
                return;
            }
        }

        if (exitCode == 131092)
        {
            var args = new VerifyCredentialsEventArgs();
            OnVerifyCredentials(args);

            if (!IsProcessGenerationCurrent(processGeneration))
                return;

            if (args.CredentialsApplied)
            {
                Configuration.Username = args.Username;
                Configuration.Domain = args.Domain;
                Configuration.Password = args.Password;
                Reconnect(processGeneration);
                return;
            }
        }

        if (!IsProcessGenerationCurrent(processGeneration))
            return;

        Configuration.DesktopWidth = _initialDesktopWidth;
        Configuration.DesktopHeight = _initialDesktopHeight;
        OnDisconnected(new DisconnectEventArgs((uint) exitCode));
    }

    private void TimerResizeInProgress_Tick(object? sender, EventArgs e)
    {
        if (DisposeStarted || Disposing || IsDisposed)
            return;

        if (MouseButtons == MouseButtons.Left)
            return;
        _timerResizeInProgress.Stop();

        // make sure that Size 0,0 (when minimized) is also ignored
        if (Size.Width == 0 ||
            Size.Height == 0 ||
            _previousClientSize.IsEmpty ||
            _previousClientSize.Equals(Size))
            return;

        Reconnect();
    }

    private void ApplyAutoScaling()
    {
        Configuration.DesktopWidth = (int) (Configuration.DesktopWidth * GetDpiScalingFactor());
        Configuration.DesktopHeight = (int) (Configuration.DesktopHeight * GetDpiScalingFactor());

        if (_initialDesktopWidth < 0)
            _initialDesktopWidth = Configuration.DesktopWidth;
        if (_initialDesktopHeight < 0)
            _initialDesktopHeight = Configuration.DesktopHeight;

        if (!Configuration.AutoScaling)
            return;

        Configuration.DesktopScaleFactor = EnsureScalingInRange(GetDpiScalingInPercent());
        Configuration.DeviceScaleFactor = Configuration.DesktopScaleFactor switch
        {
            <= 100 => 100,
            < 200 => 140,
            >= 200 => 180,
        };

        _initialZoomFactor = _currentZoomFactor = Configuration.DesktopScaleFactor;
    }

    private int EnsureScalingInRange(int scalingFactor) => scalingFactor switch
    {
        < 100 => 100,
        > 500 => 500,
        _ => scalingFactor
    };

    private void VerifyExecutable(string freeRdpPath)
    {
        if (File.Exists(freeRdpPath) && _executableWritten)
            return;

        File.WriteAllBytes(
            freeRdpPath,
            GetType().Assembly.GetResourceFileAsBytes(WFREERDP_EXE));

        _executableWritten = true;
    }

    private double GetDpiScalingFactor() => DeviceDpi / 96.0;
    private int GetDpiScalingInPercent() => (int) GetDpiScalingFactor() * 100;

    private bool StopProcess(Process? expectedProcess = null, long? expectedGeneration = null)
    {
        Process? process;
        lock (_processSyncRoot)
        {
            if (expectedProcess is not null && !ReferenceEquals(_process, expectedProcess))
                return false;

            if (expectedGeneration.HasValue && _processGeneration != expectedGeneration.Value)
                return false;

            process = _process;
            _process = null;
            unchecked
            {
                _processGeneration++;
            }
            if (process is not null)
                process.Exited -= Process_Exited;
        }

        Configuration.DesktopWidth = _initialDesktopWidth;
        Configuration.DesktopHeight = _initialDesktopHeight;
        _freeRdpWindowHandle = IntPtr.Zero;

        if (process is null)
            return true;

        try
        {
            if (process.HasExited)
                return true;

            process.Kill(entireProcessTree: true);
            if (!process.WaitForExit(PROCESS_EXIT_TIMEOUT_MILLISECONDS))
                Logger.LogWarning("wfreerdp.exe did not exit within {Timeout} ms", PROCESS_EXIT_TIMEOUT_MILLISECONDS);
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, "Killing wfreerdp.exe failed");
        }
        finally
        {
            process.Dispose();
        }

        return true;
    }

    private void OnConnected(long processGeneration)
    {
        InvokeIfLifecycleActive(() =>
        {
            if (IsProcessGenerationCurrent(processGeneration))
                Connected?.Invoke(this, EventArgs.Empty);
        });
    }

    private void OnDisconnected(DisconnectEventArgs disconnectEventArgs)
    {
        InvokeIfLifecycleActive(() => Disconnected?.Invoke(this, disconnectEventArgs));
    }

    private void OnCertificateError(CertificateErrorEventArgs certificateErrorEventArgs)
    {
        InvokeIfLifecycleActive(() => CertificateError?.Invoke(this, certificateErrorEventArgs));
    }

    private void OnVerifyCredentials(VerifyCredentialsEventArgs verifyCredentialsEventArgs)
    {
        InvokeIfLifecycleActive(() => VerifyCredentials?.Invoke(this, verifyCredentialsEventArgs));
    }

    private void InvokeIfLifecycleActive(Action action)
    {
        if (!CanRaiseLifecycleEvent)
            return;

        if (!InvokeRequired)
        {
            if (CanRaiseLifecycleEvent)
                action();
            return;
        }

        if (!IsHandleCreated)
            return;

        try
        {
            Invoke((MethodInvoker)(() =>
            {
                if (CanRaiseLifecycleEvent)
                    action();
            }));
        }
        catch (ObjectDisposedException) when (!CanRaiseLifecycleEvent || !IsHandleCreated)
        {
            // Expected when disposal wins the race with a queued process notification.
        }
        catch (InvalidOperationException) when (!CanRaiseLifecycleEvent || !IsHandleCreated)
        {
            // Expected when the handle is destroyed before the invocation can be dispatched.
        }
    }

    private void Reconnect()
    {
        StopProcess();
        Connect();
    }

    private void Reconnect(long expectedGeneration)
    {
        if (!StopProcess(expectedGeneration: expectedGeneration))
            return;

        Connect();
    }

    private bool IsProcessGenerationCurrent(long processGeneration)
    {
        lock (_processSyncRoot)
        {
            return _processGeneration == processGeneration;
        }
    }

    private void SetFocusToFreeRdpWindow()
    {
        if (!CanForwardFocus())
            return;

        if (!WindowHelper.IsFreeRdpWindow(_freeRdpWindowHandle))
            _freeRdpWindowHandle = WindowHelper.GetFreeRdpWindow(_renderTarget.Handle);

        WindowHelper.SendFocusMessage(_freeRdpWindowHandle);
    }

    private bool DisposeStarted => System.Threading.Volatile.Read(ref _disposeStarted) != 0;

    private bool CanRaiseLifecycleEvent =>
        !DisposeStarted &&
        !Disposing &&
        !IsDisposed;

    private bool CanProcessFocusMessage() =>
        !DisposeStarted &&
        !Disposing &&
        !IsDisposed &&
        IsHandleCreated;

    private bool CanForwardFocus() =>
        CanProcessFocusMessage() &&
        CanUseRenderTarget();

    private bool CanUseRenderTarget() =>
        !_renderTarget.Disposing &&
        !_renderTarget.IsDisposed &&
        _renderTarget.IsHandleCreated;
}
