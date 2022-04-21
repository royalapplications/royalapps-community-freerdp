namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// Sets and retrieves the audio redirection mode and different audio redirection options.
/// </summary>
public enum AudioRedirectionMode
{
    /// <summary>
    /// Audio redirection is not specified. The default value of wfreerdp.exe is used.
    /// </summary>
    NotSpecified = -1,
    /// <summary>
    /// Audio redirection is enabled and the option for redirection is "Bring to this computer".
    /// </summary>
    Local = 0,
    /// <summary>
    /// Audio redirection is enabled and the option is "Leave at remote computer".
    /// </summary>
    Server = 1,
    /// <summary>
    /// Audio redirection is enabled and the mode is "Do not play".
    /// </summary>
    None = 2
}