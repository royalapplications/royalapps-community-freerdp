using System;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace RoyalApps.Community.FreeRdp.WinForms;

internal static class WindowHelper
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    
    public static IntPtr GetFreeRdpWindow(IntPtr parentWindowHandle)
    {
        var returnHandle = IntPtr.Zero;
        PInvoke.EnumChildWindows(new HWND(parentWindowHandle), (hWnd, _) =>
        {
            returnHandle = hWnd;
            var sb = new StringBuilder();
            GetClassName(hWnd, sb, 7);
            return !sb.ToString().Equals("FREERDP", StringComparison.CurrentCultureIgnoreCase);
        }, new LPARAM());
        
        return returnHandle;
    }
    
    public static void SendFocusMessage(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return;

        PInvoke.SendMessage(new HWND(hWnd), PInvoke.WM_SETFOCUS, new WPARAM(0), new LPARAM(0));
    }
}