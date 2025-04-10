using System;

namespace RoyalApps.Community.FreeRdp.WinForms;

/// <summary>
/// Event arguments for the CertificateError event
/// </summary>
public class CertificateErrorEventArgs : EventArgs
{
    internal bool ShouldContinue { get; private set; }

    /// <summary>
    /// Ignores the error and continues. A new connection with /cert-ignore argument will be established.
    /// </summary>
    public void Continue()
    {
        ShouldContinue = true;
    }
}