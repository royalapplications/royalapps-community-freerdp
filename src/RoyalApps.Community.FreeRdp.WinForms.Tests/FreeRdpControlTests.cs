using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using Windows.Win32;

namespace RoyalApps.Community.FreeRdp.WinForms.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class FreeRdpControlTests
{
    [Test]
    public void FocusMessageAfterDisposeDoesNotThrowOrRecreateHandle()
    {
        var control = new TestFreeRdpControl();
        control.CreateControl();
        Assert.That(control.IsHandleCreated, Is.True);

        control.Dispose();
        Assert.That(control.IsHandleCreated, Is.False);

        Assert.That(
            () => control.DispatchMessage(PInvoke.WM_SETFOCUS),
            Throws.Nothing);
        Assert.That(control.IsHandleCreated, Is.False);
    }

    [Test]
    public void MouseActivateMessageAfterDisposeDoesNotThrowOrRecreateHandle()
    {
        var control = new TestFreeRdpControl();
        control.CreateControl();
        control.Dispose();

        Assert.That(
            () => control.DispatchMessage(PInvoke.WM_MOUSEACTIVATE),
            Throws.Nothing);
        Assert.That(control.IsHandleCreated, Is.False);
    }

    [Test]
    public void FocusMessageDuringDisposalIsIgnoredBeforeBaseDisposeStarts()
    {
        var control = new TestFreeRdpControl();
        control.CreateControl();

        var timer = GetResizeTimer(control);
        var messageDispatched = false;
        var disposingDuringMessage = false;
        var disposedDuringMessage = false;
        var handleCreatedBeforeMessage = false;
        var handleCreatedAfterMessage = false;
        var messageResult = IntPtr.Zero;

        timer.Disposed += (_, _) =>
        {
            messageDispatched = true;
            disposingDuringMessage = control.Disposing;
            disposedDuringMessage = control.IsDisposed;
            handleCreatedBeforeMessage = control.IsHandleCreated;
            messageResult = control.DispatchMessageWithResult(PInvoke.WM_MOUSEACTIVATE);
            handleCreatedAfterMessage = control.IsHandleCreated;
        };

        control.Dispose();

        Assert.Multiple(() =>
        {
            Assert.That(messageDispatched, Is.True);
            Assert.That(disposingDuringMessage, Is.False);
            Assert.That(disposedDuringMessage, Is.False);
            Assert.That(handleCreatedBeforeMessage, Is.True);
            Assert.That(messageResult, Is.EqualTo(IntPtr.Zero));
            Assert.That(handleCreatedAfterMessage, Is.True);
            Assert.That(control.IsHandleCreated, Is.False);
        });
    }

    [Test]
    public void LifecycleCallbacksAndResizeTimerRemainStoppedDuringDisposal()
    {
        var control = new TestFreeRdpControl();
        control.Configuration.SmartReconnect = true;
        control.CreateControl();

        var timer = GetResizeTimer(control);
        var disposalBoundaryReached = false;
        var timerEnabledAfterResize = true;
        var disconnectedEventRaised = false;
        control.Disconnected += (_, _) => disconnectedEventRaised = true;
        timer.Disposed += (_, _) =>
        {
            disposalBoundaryReached = true;
            control.Width++;
            timerEnabledAfterResize = timer.Enabled;
            control.Disconnect();
        };

        control.Dispose();

        Assert.Multiple(() =>
        {
            Assert.That(disposalBoundaryReached, Is.True);
            Assert.That(timerEnabledAfterResize, Is.False);
            Assert.That(disconnectedEventRaised, Is.False);
            Assert.That(control.IsDisposed, Is.True);
        });
    }

    [Test]
    public void UnrelatedChildWindowIsNotReturnedAsFreeRdpWindow()
    {
        using var parent = new Form();
        using var child = new Button {Parent = parent};
        parent.CreateControl();
        _ = child.Handle;

        Assert.That(child.IsHandleCreated, Is.True);

        var result = WindowHelper.GetFreeRdpWindow(parent.Handle);

        Assert.That(result, Is.EqualTo(IntPtr.Zero));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void InvalidWindowIsRejected(int handle)
    {
        var invalidHandle = new IntPtr(handle);

        Assert.That(WindowHelper.IsFreeRdpWindow(invalidHandle), Is.False);
        Assert.That(() => WindowHelper.SendFocusMessage(invalidHandle), Throws.Nothing);
    }

    [Test]
    public void DestroyedWindowIsRejected()
    {
        var window = new NativeWindow();
        window.CreateHandle(new CreateParams());
        var staleHandle = window.Handle;

        Assert.That(staleHandle, Is.Not.EqualTo(IntPtr.Zero));

        window.DestroyHandle();

        Assert.That(WindowHelper.IsFreeRdpWindow(staleHandle), Is.False);
        Assert.That(() => WindowHelper.SendFocusMessage(staleHandle), Throws.Nothing);
    }

    [Test]
    public void ActiveFocusMessageKeepsControlHandleAlive()
    {
        using var host = new Form();
        using var control = new TestFreeRdpControl {Parent = host};
        host.CreateControl();
        control.EnsureHandleCreated();

        Assert.That(
            () => control.DispatchMessage(PInvoke.WM_SETFOCUS),
            Throws.Nothing);
        Assert.That(control.IsHandleCreated, Is.True);
    }

    [Test]
    public void StaleProcessExitDoesNotAffectReplacementGeneration()
    {
        using var control = new TestFreeRdpControl();
        control.CreateControl();
        control.Configuration.DesktopWidth = 1234;
        control.Configuration.DesktopHeight = 567;

        var disconnectedEventRaised = false;
        control.Disconnected += (_, _) => disconnectedEventRaised = true;

        SetProcessGeneration(control, 2);
        InvokeProcessExitHandler(control, processGeneration: 1, exitCode: 0);

        Assert.Multiple(() =>
        {
            Assert.That(disconnectedEventRaised, Is.False);
            Assert.That(control.Configuration.DesktopWidth, Is.EqualTo(1234));
            Assert.That(control.Configuration.DesktopHeight, Is.EqualTo(567));
        });
    }

    private sealed class TestFreeRdpControl : FreeRdpControl
    {
        public void EnsureHandleCreated() => _ = Handle;

        public void DispatchMessage(uint messageId)
        {
            _ = DispatchMessageWithResult(messageId);
        }

        public IntPtr DispatchMessageWithResult(uint messageId)
        {
            var message = Message.Create(HandleIfCreated, (int)messageId, IntPtr.Zero, IntPtr.Zero);
            WndProc(ref message);
            return message.Result;
        }

        private IntPtr HandleIfCreated => IsHandleCreated ? Handle : IntPtr.Zero;
    }

    private static System.Windows.Forms.Timer GetResizeTimer(FreeRdpControl control)
    {
        var timerField = typeof(FreeRdpControl).GetField(
            "_timerResizeInProgress",
            BindingFlags.Instance | BindingFlags.NonPublic);

        return (System.Windows.Forms.Timer)timerField!.GetValue(control)!;
    }

    private static void SetProcessGeneration(FreeRdpControl control, long processGeneration)
    {
        var generationField = typeof(FreeRdpControl).GetField(
            "_processGeneration",
            BindingFlags.Instance | BindingFlags.NonPublic);

        generationField!.SetValue(control, processGeneration);
    }

    private static void InvokeProcessExitHandler(
        FreeRdpControl control,
        long processGeneration,
        int exitCode)
    {
        var handler = typeof(FreeRdpControl).GetMethod(
            "HandleProcessExit",
            BindingFlags.Instance | BindingFlags.NonPublic);

        handler!.Invoke(control, [processGeneration, exitCode]);
    }
}
