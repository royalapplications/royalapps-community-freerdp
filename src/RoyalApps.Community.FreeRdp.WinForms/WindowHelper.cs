using System;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace RoyalApps.Community.FreeRdp.WinForms;

internal static class WindowHelper
{
    private const string FREERDP_WINDOW_CLASS_NAME = "FREERDP";

    public static IntPtr GetFreeRdpWindow(IntPtr parentWindowHandle)
    {
        var returnHandle = IntPtr.Zero;
        PInvoke.EnumChildWindows(new HWND(parentWindowHandle), (hWnd, _) =>
        {
            if (!IsFreeRdpWindow(hWnd))
                return true;

            returnHandle = hWnd;
            return false;
        }, new LPARAM());

        return returnHandle;
    }

    public static bool IsFreeRdpWindow(IntPtr hWnd)
    {
        var windowHandle = new HWND(hWnd);
        if (windowHandle.IsNull || !PInvoke.IsWindow(windowHandle))
            return false;

        Span<char> className = stackalloc char[FREERDP_WINDOW_CLASS_NAME.Length + 1];
        var classNameLength = PInvoke.GetClassName(windowHandle, className);
        return classNameLength > 0 &&
               MemoryExtensions.Equals(
                   className[..classNameLength],
                   FREERDP_WINDOW_CLASS_NAME.AsSpan(),
                   StringComparison.OrdinalIgnoreCase);
    }

    public static void SendFocusMessage(IntPtr hWnd)
    {
        if (!IsFreeRdpWindow(hWnd))
            return;

        PInvoke.SendMessage(new HWND(hWnd), PInvoke.WM_SETFOCUS, new WPARAM(0), new LPARAM(0));
    }
}
