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
    public uint ExitCode { get; }

    /// <summary>
    /// Whether or not the disconnected by the user directly.
    /// </summary>
    public bool UserInitiated { get; set; }

    /// <summary>
    /// The error message from exit code.
    /// </summary>
    public string ErrorMessage { get; }
    
    /// <summary>
    /// Creates a new event arg instance.
    /// </summary>
    /// <param name="exitCode">The exit code of wfreerdp.exe</param>
    public DisconnectEventArgs(uint exitCode)
    {
        ExitCode = exitCode;
        var error = RdpErrorInfo.ErrorInfoFromErrorCode(exitCode);
        UserInitiated = RdpErrorInfo.IsUserTriggeredDisconnectError(exitCode);
        ErrorMessage = error == null 
            ? $"Exit code: {exitCode}" 
            : error.ToString();
    }
}