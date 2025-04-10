using System.ComponentModel;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// GDI rendering mode.
/// </summary>
public enum GDIRendering
{
    /// <summary>
    /// No rendering mode is specified. The default value of wfreerdp.exe is used.
    /// </summary>
    NotSpecified,
    /// <summary>
    /// Software rendering mode.
    /// </summary>
    [Description("sw")] Software,
    /// <summary>
    /// If set, hardware assist with graphics decoding is attempted.
    /// </summary>
    [Description("hw")] Hardware
}