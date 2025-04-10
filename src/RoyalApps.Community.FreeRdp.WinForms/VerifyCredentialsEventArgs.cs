using System;

namespace RoyalApps.Community.FreeRdp.WinForms;

/// <summary>
/// Event arguments for the VerifyCredentials event
/// </summary>
public class VerifyCredentialsEventArgs : EventArgs
{
    internal bool CredentialsApplied { get; private set; }
    internal string? Username { get; private set; }
    internal string? Domain { get; private set; }
    internal string? Password { get; private set; }
    
    /// <summary>
    /// Set a username, domain and a password and try the connection again
    /// </summary>
    public void SetCredentials(string? username, string? domain, string? password)
    {
        Username = username;
        Domain = domain;
        Password = password;
        CredentialsApplied = true;
    }
}