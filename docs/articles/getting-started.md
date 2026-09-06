# Getting Started

## Requirements

Use a Windows WinForms application targeting `net10.0-windows` and the .NET 10 SDK. See the [Support Matrix](/articles/support-matrix) for native-client and architecture considerations.

## Install

```sh
dotnet add package RoyalApps.Community.FreeRdp.WinForms
```

Alternatively, in Visual Studio's NuGet Package Manager Console:

```powershell
Install-Package RoyalApps.Community.FreeRdp.WinForms
```

The [diagnostics API](/articles/diagnostics) requires version 2.2.4 or later. If that version is not yet available in your feed, use a locally built package.

## Add the control

Create and connect the control on the WinForms UI thread. Add it to a form or container before connecting, so that it has a window handle and a usable client size.

This form accepts credentials obtained by the host; it does not store example passwords in source:

```csharp
using System.Windows.Forms;
using RoyalApps.Community.FreeRdp.WinForms;

public sealed class RemoteDesktopForm : Form
{
    private readonly FreeRdpControl _rdp = new() { Dock = DockStyle.Fill };

    public RemoteDesktopForm(string server, string username, string password)
    {
        Text = "Remote Desktop";
        ClientSize = new System.Drawing.Size(1280, 800);
        Controls.Add(_rdp);

        _rdp.Configuration.Server = server;
        _rdp.Configuration.Username = username;
        _rdp.Configuration.Password = password;

        Shown += (_, _) => _rdp.Connect();
    }
}
```

The form owns its child control and disposes it when the form is disposed. Disposing the control stops its native client.

## Configure and connect

Set `Configuration.Domain` and `Configuration.Port` if needed. `Server` is required; the credentials needed for authentication depend on your server and security configuration.

`Connect()` validates configuration and can throw, for example for invalid values or process-start failures. Your host should catch these errors at the UI boundary and report a sanitized message.

For a normal disconnect:

```csharp
rdp.Disconnect();
```

This terminates the associated client process. It does not request a Windows user logoff on the remote server.

::: warning Certificate validation
Use a server hostname matching its certificate and a trusted certificate chain. Do not disable certificate validation as a general connection workaround. See [Lifecycle and Events](/articles/lifecycle#certificate-errors) before handling certificate errors.
:::

## Explore the demo

Open `src/RoyalApps.Community.FreeRdp.slnx` and run the `RoyalApps.Community.FreeRdp.WinForms.Demo` project.

The **Connection** menu offers **Connect**, **Disconnect**, and **Settings**. Configure the server and credentials before connecting; settings expose the configuration object's properties.

Continue with [Configuration](/articles/configuration) and [Lifecycle and Events](/articles/lifecycle).

