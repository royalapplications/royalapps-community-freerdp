using System;
using System.Runtime.InteropServices;

namespace RoyalApps.Community.FreeRdp.WinForms.Extensions;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct CREDUI_INFO
{
    public int cbSize;
    public IntPtr hwndParent;
    public string pszMessageText;
    public string pszCaptionText;
    public IntPtr hbmBanner;
}
