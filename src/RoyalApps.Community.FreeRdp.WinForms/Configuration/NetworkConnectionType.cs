using System.ComponentModel;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// Gets or sets the type of network connection used between the client and server. The network connection type information passed on to the server helps the server tune several parameters based on the network connection type.
/// </summary>
public enum NetworkConnectionType
{
    /// <summary>
    /// Network connection type is not specified. The default value of wfreerdp.exe is used.
    /// </summary>
    NotSpecified,
    /// <summary>
    /// Modem (56 Kbps).
    /// </summary>
    [Description("modem")] Modem,
    /// <summary>
    /// Low-speed broadband (256 Kbps to 2 Mbps).
    /// </summary>
    [Description("broadband-low")] BroadbandLow,
    /// <summary>
    /// Satellite (2 Mbps to 16 Mbps, with high latency).
    /// </summary>
    [Description("broadband")] Broadband,
    /// <summary>
    /// High-speed broadband (2 Mbps to 10 Mbps).
    /// </summary>
    [Description("broadband-high")] BroadbandHigh,
    /// <summary>
    /// Wide area network (WAN) (10 Mbps or higher, with high latency).
    /// </summary>
    [Description("wan")] WAN,
    /// <summary>
    /// Local area network (LAN) (10 Mbps or higher).
    /// </summary>
    [Description("lan")] LAN,
    /// <summary>
    /// Tries to detect the network connection type automatically.
    /// </summary>
    [Description("auto")] Autodetect
}