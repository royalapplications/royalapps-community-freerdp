# Lifecycle and Events

The host owns the `FreeRdpControl`. Keep UI operations on the WinForms UI thread and dispose the control when its container is no longer needed.

## Connection events

| Event | Meaning |
| --- | --- |
| `Connected` | The control has detected and embedded the native client's window. This is not a server-side authentication audit signal. |
| `Disconnected` | A terminal exit was processed; inspect `ExitCode`, `ErrorMessage`, and `UserInitiated` on `DisconnectEventArgs`. |
| `CertificateError` | The native client reported a certificate error that the wrapper can offer to retry. |
| `VerifyCredentials` | The native client reported an authentication error that the wrapper can offer to retry. |
| `DiagnosticOutput` | A raw native output or lifecycle record; requires diagnostics to be enabled for that launch. |

Subscribe before connecting. Diagnostics handlers have different threading and shutdown requirements; see [Diagnostics](/articles/diagnostics).

## Certificate errors

`CertificateErrorEventArgs.Continue()` authorizes retrying with certificate checks disabled. Do not call it unconditionally.

Present an appropriate security decision in your host. If the user rejects the exception, leave the event arguments unchanged. The event itself does not expose a certificate chain or fingerprint for independent verification.

The current implementation recognizes native exit code `131080` for this retry path. A custom client can report different failure codes.

## Credential retries

On `VerifyCredentials`, prompt using your host's secure credential UI. If the user confirms, call:

```csharp
e.SetCredentials(username, domain, password);
```

The wrapper updates the destination credential fields and launches a new client. Leave the event arguments unchanged when the user cancels. The current retry path recognizes native exit code `131092`.

Do not log authentication responses. The host is responsible for preventing endless interactive retries.

## Disconnect, reconnect, and disposal

`Disconnect()` stops the child process. It does not log off the remote Windows account. A later `Connect()` launches a new client. Calling `Connect()` while the current client is still running does not launch a duplicate.

Smart resize, zoom changes, and accepted certificate or credential retries can also launch replacement processes. With diagnostics enabled, each process gets its own `LaunchId`, even when it belongs to the same host connection attempt.

Disposal prevents further connection activity. Diagnostic stream cleanup is asynchronous and bounded; final output can arrive after the ordinary connection events or disposal. Do not have diagnostic handlers access disposed controls. Retain the logging sink long enough to accept and flush trailing records.

Cross-stream stdout/stderr ordering is not guaranteed. Use launch identifiers for correlation and add receive timestamps in the host.

