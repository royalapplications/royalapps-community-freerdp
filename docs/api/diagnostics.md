# Diagnostics Types

Namespace: `RoyalApps.Community.FreeRdp.WinForms`

Introduced in **2.2.4** without changing existing public signatures.

This page explains the diagnostics contract. Member-level documentation is generated for [FreeRdpDiagnosticsOptions](/api/reference/royalapps-community-freerdp-winforms-freerdpdiagnosticsoptions), [FreeRdpDiagnosticEventArgs](/api/reference/royalapps-community-freerdp-winforms-freerdpdiagnosticeventargs), and [FreeRdpControl](/api/reference/royalapps-community-freerdp-winforms-freerdpcontrol).

## FreeRdpDiagnosticsOptions

```csharp
public sealed class FreeRdpDiagnosticsOptions
{
    public string LogLevel { get; set; } = "DEBUG";
}
```

Supported levels are `TRACE`, `DEBUG`, `INFO`, `WARN`, and `ERROR`, case-insensitively. Unknown values normalize to `DEBUG`.

Assign `FreeRdpControl.DiagnosticsOptionsProvider` to return a fresh options snapshot for the next native launch. Return null for no wrapper-managed capture on that launch.

There is no package-level log-file setting or global enable switch. The host owns these policies.

## FreeRdpDiagnosticEventArgs

| Property | Type | Meaning |
| --- | --- | --- |
| `LaunchId` | `Guid` | Unique native-process launch identifier. |
| `ProcessId` | `int` | Native child PID. |
| `Source` | `string` | `stdout`, `stderr`, or `lifecycle`. |
| `Message` | `string` | Raw text; may contain sensitive data. |

Properties are init-only. There is no timestamp, parsed severity, or host connection ID in this event. Add those in your logging coordinator. Do not assume stderr always means an error; native text can include its own severity.

Lifecycle text currently includes executable/version metadata and process exit codes. Treat that text as human-readable diagnostics, not a stable machine-readable protocol.

Output is drained concurrently, with individual lines bounded to 65,536 characters. Oversized messages are omitted; final unterminated lines are delivered when a stream ends. Cleanup waits for readers asynchronously with a bounded timeout, so abrupt termination cannot guarantee a complete trace.

See [Diagnostics](/articles/diagnostics) for integration, threading, and security requirements.

[Diagnostics source](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/FreeRdpDiagnosticsOptions.cs).
