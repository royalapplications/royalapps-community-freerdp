# Diagnostics

Version **2.2.4** adds opt-in, per-launch capture through `DiagnosticsOptionsProvider` and `DiagnosticOutput`. It captures the native process independently of the wrapper's ordinary `ILogger`.

::: warning Sensitive data
Native stdout and stderr are raw, unredacted output. They may contain credentials, gateway tokens, or other connection data. Never write them directly to a log file or upload them without a host-owned sanitization policy.
:::

## Enable capture before each launch

Assign a provider before the first `Connect()`:

```csharp
rdp.DiagnosticsOptionsProvider = () => new FreeRdpDiagnosticsOptions
{
    LogLevel = "DEBUG"
};
```

The provider runs before **every child-process launch**, including wrapper-level reconnects, zoom/resize restarts, and accepted certificate or credential retries. A native reconnect within the same still-running process is not a new wrapper launch.

Return null to leave that launch's native diagnostics unchanged:

```csharp
rdp.DiagnosticsOptionsProvider = () => null;
```

That does not retroactively turn off output redirection for an existing process, nor does it remove logging settings supplied externally in the native environment or additional arguments.

Keep the provider quick, thread-safe, and free of UI interaction or file I/O. Return a new options snapshot rather than sharing a mutable options instance.

## Route records to a host-owned coordinator

The following integration uses an application-defined sink. It intentionally does not implement a file writer or a generic sanitizer in the wrapper:

```csharp
using System;
using RoyalApps.Community.FreeRdp.WinForms;

public interface IFreeRdpDiagnosticSink
{
    bool Enabled { get; }
    string LogLevel { get; }

    // Must sanitize, apply the current enablement policy, and enqueue without
    // blocking. Return false if disabled or the bounded queue is full.
    bool TryEnqueueRedacted(DateTimeOffset receivedUtc, FreeRdpDiagnosticEventArgs record);
}

public static class FreeRdpDiagnosticRegistration
{
    public static void Attach(FreeRdpControl control, IFreeRdpDiagnosticSink sink)
    {
        control.DiagnosticsOptionsProvider = () => sink.Enabled
            ? new FreeRdpDiagnosticsOptions { LogLevel = sink.LogLevel }
            : null;

        control.DiagnosticOutput += (_, record) =>
        {
            if (sink.Enabled)
                sink.TryEnqueueRedacted(DateTimeOffset.UtcNow, record);
        };
    }
}
```

Use one shared coordinator across your application's controls if you want plugin-wide settings and a shared file. Attach once per control. Keep the coordinator alive until native output has drained; avoid capturing a form or accessing disposed UI from the output handler.

The sink should:

- enforce capture enablement at the point a record is accepted, including races with disabling;
- redact known passwords/tokens and recognized secret fields before persisting records;
- use a bounded, nonblocking queue and report dropped-record counts;
- write in the background, serialize concurrent writers, and handle invalid paths without failing a connection;
- add UTC timestamps, host attempt IDs, severity where recognizable, and the event's PID and launch ID;
- flush on normal application shutdown and define log retention and access permissions.

Do not log complete configuration objects, command lines, authentication responses, or clipboard content. Keep a sensitive-data warning even after implementing redaction: arbitrary native output cannot be assumed safe.

## Threading and stream lifetime

`DiagnosticOutput` handlers must be thread-safe and must not block. They can run on background reader threads; lifecycle records can also be delivered on the launch path. Do not synchronously invoke the UI or perform file I/O inside a handler.

Stdout and stderr are redirected and drained concurrently. Each record includes stream identity, native PID, and a unique launch ID. Output from the two streams can interleave, and old-process output can overlap a replacement client's output.

On exit, reconnect, or disposal, reader cleanup is asynchronous and bounded. Final unterminated lines are captured when a stream ends normally. Slow readers or abrupt process termination can still cause incomplete final capture; the cleanup path must not hold up the control's window lifecycle.

The event does not promise a stable lifecycle-message format. Use `LaunchId` and `ProcessId` for correlation, not string parsing to manage control ownership.

## Child-only configuration and precedence

For a diagnostics-enabled launch, the wrapper:

1. normalizes the requested level to `TRACE`, `DEBUG`, `INFO`, `WARN`, or `ERROR` (default `DEBUG`);
2. replaces conflicting `/log-level:`, `/log-filters:`, and `/log-appender:` arguments while retaining unrelated arguments;
3. sets the child's `WLOG_LEVEL` and `WLOG_APPENDER=CONSOLE`, and removes its inherited `WLOG_FILTER`;
4. redirects both output streams.

It does **not** change the hosting application's process-wide environment. Different controls can select different diagnostic levels on their next launch.

Changing the provider's result requires a new process launch to change native verbosity or start redirection. The wrapper no longer emits a full-command-line Trace record; it logs executable/launch metadata instead. This does not hide the operating system's process command line from tools with permission to inspect it.

## Disabling and changing files

A host can immediately stop accepting diagnostic records while continuing to drain an already redirected child process. Never stop reading redirected pipes merely because capture has been disabled.

File paths belong to the host's coordinator, so the host can route new accepted records to a new file without reconnecting. Records already queued for the old destination may still flush there.

The wrapper does not impose file defaults, environment-variable expansion for log files, application Trace precedence, startup reset, or persistence. Those are application policies.

### Royal TS integration policy

The Royal TS V7 integration uses this wrapper with plugin-wide settings. Enablement is reset on application startup, while level and path are retained; the default file is `%TEMP%\RoyalTS-FreeRDP-V7.log`. Royal TS application Trace logging does not automatically enable the diagnostic file.

Enabling or changing verbosity requires reconnecting for complete capture. Disabling stops new file capture; changing the path redirects newly accepted records. These behaviors are supplied by Royal TS, not by the NuGet package.

Royal TS V26 uses FreeRdpKit instead and is outside this wrapper's implementation.

See [Diagnostics Types](/api/diagnostics) for the public API.

