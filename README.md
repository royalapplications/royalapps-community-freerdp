# FreeRDP Control

[![NuGet Version](https://img.shields.io/nuget/v/RoyalApps.Community.FreeRdp.WinForms.svg?style=flat)](https://www.nuget.org/packages/RoyalApps.Community.FreeRdp.WinForms)
[![NuGet Downloads](https://img.shields.io/nuget/dt/RoyalApps.Community.FreeRdp.WinForms.svg?color=green)](https://www.nuget.org/packages/RoyalApps.Community.FreeRdp.WinForms)
[![.NET](https://img.shields.io/badge/.NET-net10.0--windows-blueviolet)](https://dotnet.microsoft.com/download)

`RoyalApps.Community.FreeRdp.WinForms` embeds [FreeRDP's Windows client](https://github.com/FreeRDP/FreeRDP) in a WinForms application. It launches `wfreerdp.exe` with a parent-window handle; it does not use Microsoft RDP ActiveX or FreeRdpKit.

- Typed destination, gateway, display, certificate, and redirection settings.
- A bundled client, with support for selecting a compatible custom executable.
- Connection events, credential/certificate retries, and resize/zoom reconnects.
- Opt-in native diagnostics with per-launch configuration and stdout/stderr capture, introduced in **2.2.4**.

The current source targets **.NET 10 on Windows**. Older package releases may have different framework requirements.

## Documentation

The VitePress documentation lives in [`docs/`](docs/index.md):

- [Getting Started](docs/articles/getting-started.md)
- [Configuration](docs/articles/configuration.md)
- [Lifecycle and Events](docs/articles/lifecycle.md)
- [Diagnostics](docs/articles/diagnostics.md)
- [Troubleshooting](docs/articles/troubleshooting.md)
- [Support Matrix](docs/articles/support-matrix.md)
- [API Reference](docs/api/index.md)

See [documentation development and deployment](docs/articles/contributing.md) to preview the site locally or publish it to GitHub Pages.

![FreeRDP demo](https://raw.githubusercontent.com/royalapplications/royalapps-community-freerdp/main/docs/assets/Screenshot.png)

## Installation

```sh
dotnet add package RoyalApps.Community.FreeRdp.WinForms
```

For the diagnostics APIs, install version 2.2.4 or later from a feed containing that version.

## Quick start

On the WinForms UI thread, add the control to a form or container before connecting:

```csharp
using System.Windows.Forms;
using RoyalApps.Community.FreeRdp.WinForms;

// In your form's constructor, after initialization:
var rdp = new FreeRdpControl { Dock = DockStyle.Fill };
Controls.Add(rdp);
rdp.Configuration.Server = "desktop.example.test";

// Obtain credentials through your host application's secure credential flow.
rdp.Configuration.Username = username;
rdp.Configuration.Password = password;
Shown += (_, _) => rdp.Connect();
```

Use `rdp.Disconnect()` to stop the client. The containing form owns and disposes the control. Keep certificate validation enabled; see the guide before implementing certificate exceptions.

## Diagnostics

Set `DiagnosticsOptionsProvider` before connecting and subscribe to `DiagnosticOutput`. The provider is evaluated before every native launch, including internal retries.

Output is **raw and potentially sensitive**. Handlers must be thread-safe and nonblocking. The host owns redaction, bounded buffering, file writing, persistence, and enable/disable policy. See the [diagnostics guide](docs/articles/diagnostics.md) for an integration example and lifecycle details.

## Demo and development

Open `src/RoyalApps.Community.FreeRdp.slnx` and run `RoyalApps.Community.FreeRdp.WinForms.Demo`.

```sh
dotnet build src/RoyalApps.Community.FreeRdp.slnx -c Release
dotnet test src/RoyalApps.Community.FreeRdp.WinForms.Tests/RoyalApps.Community.FreeRdp.WinForms.Tests.csproj -c Release
npm ci
npm run docs:dev
```

## License and acknowledgements

[MIT License](LICENSE). FreeRDP and other bundled dependencies retain their respective licenses.

Special thanks to [Marc-André Moreau](https://github.com/awakecoding) and [akallabeth](https://github.com/akallabeth) for their help.
