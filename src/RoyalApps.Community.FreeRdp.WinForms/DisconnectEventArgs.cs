using System;

namespace RoyalApps.Community.FreeRdp.WinForms;

/// <summary>
/// Contains disconnect information.
/// </summary>
public class DisconnectEventArgs : EventArgs
{
    /// <summary>
    /// The exit code of wfreerdp.exe
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Creates a new event arg instance.
    /// </summary>
    /// <param name="exitCode">The exit code of wfreerdp.exe</param>
    public DisconnectEventArgs(int exitCode)
    {
        ExitCode = exitCode;
    }
}