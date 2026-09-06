using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalApps.Community.FreeRdp.WinForms;

/// <summary>Diagnostic settings sampled before each native client launch.</summary>
public sealed class FreeRdpDiagnosticsOptions
{
    /// <summary>Minimum native severity: TRACE, DEBUG, INFO, WARN, or ERROR.</summary>
    public string LogLevel { get; set; } = "DEBUG";
}

/// <summary>A native output or lifecycle record. Callbacks must not block and may run on background threads.</summary>
public sealed class FreeRdpDiagnosticEventArgs : EventArgs
{
    /// <summary>Unique identifier for this native process launch.</summary>
    public Guid LaunchId { get; init; }
    /// <summary>Native process identifier.</summary>
    public int ProcessId { get; init; }
    /// <summary>Record source: stdout, stderr, or lifecycle.</summary>
    public string Source { get; init; } = string.Empty;
    /// <summary>Message text; may contain sensitive connection data.</summary>
    public string Message { get; init; } = string.Empty;
}

internal sealed class FreeRdpProcessDiagnostics
{
    private readonly Action<FreeRdpDiagnosticEventArgs> _output;
    private readonly Guid _launchId = Guid.NewGuid();
    private readonly int _pid;
    private readonly StreamReader _stdout;
    private readonly StreamReader _stderr;
    private readonly Task _readers;
    private int _released;

    internal static void Configure(ProcessStartInfo info, FreeRdpDiagnosticsOptions options)
    {
        var level = options.LogLevel?.ToUpperInvariant() switch
        {
            "TRACE" => "TRACE", "INFO" => "INFO", "WARN" => "WARN", "ERROR" => "ERROR", _ => "DEBUG"
        };
        // Logging options belong to this launch. Preserve all unrelated arguments.
        info.Arguments = Regex.Replace(info.Arguments,
            @"(?i)(?<!\S)/log-(?:level|filters|appender):(?:""[^""]*""|\S+)", string.Empty,
            RegexOptions.None, TimeSpan.FromMilliseconds(100)).Trim() + " /log-level:" + level;
        info.Environment["WLOG_LEVEL"] = level;
        info.Environment["WLOG_APPENDER"] = "CONSOLE";
        info.Environment.Remove("WLOG_FILTER");
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
    }

    internal FreeRdpProcessDiagnostics(Process process, Action<FreeRdpDiagnosticEventArgs> output)
    {
        _output = output;
        _pid = process.Id;
        _stdout = process.StandardOutput;
        _stderr = process.StandardError;
        string? version = null;
        try { version = FileVersionInfo.GetVersionInfo(process.StartInfo.FileName).FileVersion; }
        catch { /* Metadata lookup is optional, including executables resolved through PATH. */ }
        Emit("lifecycle", $"Client started; executable={Path.GetFileName(process.StartInfo.FileName)}; version={version ?? "unknown"}");
        _readers = Task.Run(() => Task.WhenAll(ReadAsync(_stdout, "stdout"), ReadAsync(_stderr, "stderr")));
    }

    private async Task ReadAsync(StreamReader reader, string source)
    {
        try
        {
            // Bound individual records even for clients emitting unterminated output.
            var buffer = new char[4096];
            var line = new System.Text.StringBuilder();
            var oversized = false;
            int count;
            while ((count = await reader.ReadAsync(buffer.AsMemory()).ConfigureAwait(false)) != 0)
                for (var i = 0; i < count; i++)
                {
                    var ch = buffer[i];
                    if (ch == '\n')
                    {
                        Emit(source, oversized ? "[Oversized native message omitted]" : line.ToString().TrimEnd('\r'));
                        line.Clear();
                        oversized = false;
                    }
                    else if (!oversized)
                    {
                        if (line.Length >= 65536) { line.Clear(); oversized = true; }
                        else line.Append(ch);
                    }
                }
            if (line.Length > 0 || oversized)
                Emit(source, oversized ? "[Oversized native message omitted]" : line.ToString().TrimEnd('\r'));
        }
        catch (ObjectDisposedException) { }
        catch (IOException) { }
    }

    private void Emit(string source, string message)
    {
        try { _output(new FreeRdpDiagnosticEventArgs { LaunchId = _launchId, ProcessId = _pid, Source = source, Message = message }); }
        catch { /* Subscriber failures must not affect native client lifetime. */ }
    }

    internal void Release(Process process)
    {
        if (Interlocked.Exchange(ref _released, 1) != 0) return;
        _ = Task.Run(async () =>
        {
            try
            {
                if (await Task.WhenAny(_readers, Task.Delay(3000)).ConfigureAwait(false) != _readers)
                    Emit("lifecycle", "Timed out draining native diagnostic output.");
                if (process.HasExited) Emit("lifecycle", $"Client exited; exitCode={process.ExitCode}");
            }
            catch { }
            finally
            {
                _stdout.Dispose();
                _stderr.Dispose();
                process.Dispose();
            }
        });
    }
}
