# FreeRdpControl

Namespace: `RoyalApps.Community.FreeRdp.WinForms`

This page is a usage overview. See the [generated FreeRdpControl reference](/api/reference/royalapps-community-freerdp-winforms-freerdpcontrol) for member-level documentation and linked types.

A WinForms `UserControl` that owns a native FreeRDP child process.

## Properties

| Property | Type | Purpose |
| --- | --- | --- |
| `Configuration` | `FreeRdpConfiguration` | Typed settings used on launch. Initialized by default. |
| `Logger` | `ILogger` | Ordinary wrapper logging, separate from raw diagnostic output. |
| `DiagnosticsOptionsProvider` | `Func<FreeRdpDiagnosticsOptions?>?` | Optional callback sampled before every child launch; null result leaves diagnostics unchanged. |

## Methods

| Method | Purpose |
| --- | --- |
| `Connect()` | Validate settings and start/embed the client; can throw on validation or startup failures. |
| `Disconnect()` | Stop the associated process. |
| `ZoomIn()`, `ZoomOut()` | Change scale by reconnecting. |
| `ResetZoom()` | Restore zoom through the control's reconnect behavior. |
| `SetZoomLevel(int scalingInPercent)` | Set scale and reconnect. |
| `Dispose()` | Dispose the control and stop its native client. |

## Events and arguments

- `Connected`: `EventHandler`.
- `Disconnected`: `EventHandler<DisconnectEventArgs>`; arguments expose `uint ExitCode`, `string ErrorMessage`, and settable `bool UserInitiated`.
- `CertificateError`: `EventHandler<CertificateErrorEventArgs>`; `Continue()` requests a certificate-bypassing retry.
- `VerifyCredentials`: `EventHandler<VerifyCredentialsEventArgs>`; `SetCredentials(string? username, string? domain, string? password)` requests a retry with destination credentials.
- `DiagnosticOutput`: `EventHandler<FreeRdpDiagnosticEventArgs>`; [diagnostic records](/api/diagnostics) may arrive on background threads and during asynchronous cleanup.

Read [Lifecycle and Events](/articles/lifecycle) for ordering, retry, and ownership guidance.

[FreeRdpControl source](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/FreeRdpControl.cs).
