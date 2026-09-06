# Configuration

Set `FreeRdpControl.Configuration` before connecting. Most changes are passed to the native client on the next launch, not applied to an already running process.

## Destination and gateway

The destination and RD Gateway have separate credential fields:

```csharp
rdp.Configuration.Server = "desktop.example.test";
rdp.Configuration.Port = 3389;
rdp.Configuration.Domain = "DESTINATION";
rdp.Configuration.Username = destinationUsername;
rdp.Configuration.Password = destinationPassword;

rdp.Configuration.Gateway.Hostname = "gateway.example.test";
rdp.Configuration.Gateway.Port = 443;
rdp.Configuration.Gateway.Domain = "GATEWAY";
rdp.Configuration.Gateway.Username = gatewayUsername;
rdp.Configuration.Gateway.Password = gatewayPassword;
```

The username and password variables above are supplied by the host. A null or empty gateway hostname disables gateway arguments. Gateway transport and other native extensions can be supplied through `Gateway.AdditionalArguments`; supported values depend on the selected executable.

The wrapper does not resolve Royal TS objects, inheritance, or stored-credential references. Hosts must provide the final effective values.

## Desktop size and scaling

| Setting | Behavior |
| --- | --- |
| `DesktopWidth`, `DesktopHeight` | Zero requests initial sizing from the control's client area. |
| `AutoScaling` | Defaults to true; derives initial scaling from the control's DPI. |
| `DesktopScaleFactor` | Desktop scaling percentage, 100–500. |
| `DeviceScaleFactor` | Must be 100, 140, or 180. |
| `SmartReconnect` | Reconnects after the container size changes to adopt a new desktop size. |

To choose explicit initial scaling, disable `AutoScaling`. The usual device scale pairing is 100 at desktop scale 100, 140 above 100 and below 200, and 180 at 200 or above.

The control provides `ZoomIn()`, `ZoomOut()`, `ResetZoom()`, and `SetZoomLevel(int scalingInPercent)`. These changes reconnect by restarting the child process; they are not an uninterrupted in-session zoom.

## Native executable

The bundled client is extracted to `Configuration.TempPath`, which defaults to `%temp%`. The wrapper expands environment variables for the executable and temporary directory.

Set `Configuration.Executable` to an existing compatible Windows FreeRDP executable to use a custom client. Ensure its supporting files are available and its architecture and command-line options work on the target machine. This is not an in-process native library path.

## Certificates, redirection, and security

Use the nested `Certificate`, `Cache`, `Proxy`, and `Security` objects for their corresponding settings. Common top-level settings include `Clipboard`, `AudioRedirection`, `ColorDepth`, `Network`, and `RestrictedAdminMode`.

`Certificate.Ignore` bypasses certificate checks. Keep it false unless a deliberate, user-approved exception is appropriate. A retry through `CertificateErrorEventArgs.Continue()` sets it to true on the configuration; it remains true until the host changes it.

## Additional arguments

`Configuration.AdditionalArguments` passes extra native arguments. Prefer typed properties where available, and never log complete arguments or configuration objects: they can contain passwords and gateway tokens.

When [diagnostic capture](/articles/diagnostics) is enabled for a launch, the wrapper overrides conflicting `/log-level:`, `/log-filters:`, and `/log-appender:` arguments. Other arguments are retained.

See [Configuration Types](/api/configuration) for the main API groups and source references.

