namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The color depth (in bits per pixel) for the control's connection.
/// </summary>
public enum BitsPerPixel
{
    /// <summary>
    /// No color depth specified. The default value of wfreerdp.exe is used.
    /// </summary>
    NotSpecified = 0,
    /// <summary>
    /// 8 bits per pixel
    /// </summary>
    HighColor8Bpp = 8,
    /// <summary>
    /// 16 bits per pixel
    /// </summary>
    HighColor16Bpp = 16,
    /// <summary>
    /// 24 bits per pixel
    /// </summary>
    HighColor24Bpp = 24,
    /// <summary>
    /// 32 bits per pixel
    /// </summary>
    HighColor32Bpp = 32
}