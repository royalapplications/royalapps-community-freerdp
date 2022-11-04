﻿namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The proxy mode for the FreeRdp connection
/// </summary>
public enum ProxyMode
{
    /// <summary>
    /// No proxy will be used (default)
    /// </summary>
    None,
    /// <summary>
    /// Use HTTP proxy
    /// </summary>
    Http,
    /// <summary>
    /// Use SOCKS5 proxy
    /// </summary>
    Socks5
}