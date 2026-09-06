using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RoyalApps.Community.FreeRdp.WinForms.Tests;

[TestFixture]
public class FreeRdpDiagnosticsTests
{
    [Test]
    public void ConfiguresOnlyChildEnvironmentAndPreservesUnrelatedArguments()
    {
        var parent = Environment.GetEnvironmentVariable("WLOG_LEVEL");
        var start = new ProcessStartInfo { Arguments = "/v:server /log-level:INFO /log-filters:\"*:WARN\" /u:\"test user\"" };
        FreeRdpProcessDiagnostics.Configure(start, new FreeRdpDiagnosticsOptions { LogLevel = "trace" });
        Assert.Multiple(() =>
        {
            Assert.That(start.Environment["WLOG_LEVEL"], Is.EqualTo("TRACE"));
            Assert.That(start.Environment["WLOG_APPENDER"], Is.EqualTo("CONSOLE"));
            Assert.That(start.Arguments, Does.Contain("/v:server").And.Contain("/u:\"test user\"").And.Contain("/log-level:TRACE"));
            Assert.That(start.Arguments, Does.Not.Contain("/log-filters:").And.Not.Contain("/log-level:INFO"));
            Assert.That(Environment.GetEnvironmentVariable("WLOG_LEVEL"), Is.EqualTo(parent));
            Assert.That(start.RedirectStandardError && start.RedirectStandardOutput, Is.True);
        });
    }

    [Test]
    public async Task DrainsBothStreamsAndReportsFinalOutputBeforeExit()
    {
        var records = new ConcurrentQueue<FreeRdpDiagnosticEventArgs>();
        var exited = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var process = new Process
        {
            StartInfo = new ProcessStartInfo("cmd.exe", "/d /c \"echo stdout-line & echo stderr-line 1>&2\"")
            {
                UseShellExecute = false, CreateNoWindow = true,
                RedirectStandardOutput = true, RedirectStandardError = true
            }
        };
        process.Start();
        var capture = new FreeRdpProcessDiagnostics(process, record =>
        {
            records.Enqueue(record);
            if (record.Message.StartsWith("Client exited;")) exited.TrySetResult();
        });
        await process.WaitForExitAsync();
        capture.Release(process);
        capture.Release(process);
        await exited.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.Multiple(() =>
        {
            Assert.That(records.Any(record => record.Source == "stdout" && record.Message.Contains("stdout-line")), Is.True);
            Assert.That(records.Any(record => record.Source == "stderr" && record.Message.Contains("stderr-line")), Is.True);
            Assert.That(records.Select(record => record.LaunchId).Distinct().Count(), Is.EqualTo(1));
            Assert.That(records.Count(record => record.Message.StartsWith("Client exited;")), Is.EqualTo(1));
        });
    }
}

