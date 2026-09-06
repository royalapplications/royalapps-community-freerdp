# Support Matrix

These docs describe the current repository source, not every historical NuGet release.

| Area | Current scope |
| --- | --- |
| Managed framework | `net10.0-windows`; .NET 10 SDK for building. |
| UI | Windows Forms. |
| Backend | An out-of-process Windows `wfreerdp.exe` embedded using its parent-window handle. |
| Native client | Bundled executable, or a compatible custom executable selected by the host. |
| Architecture | Project RID selections exist for x64 and ARM64. The actual executable must run on the target OS; selecting a managed RID does not generate a different FreeRDP binary. |
| Diagnostics | Additive provider and output-event APIs introduced in 2.2.4. |
| Earlier .NET / .NET Framework | Not targeted by the current project. Inspect the source and dependencies of an older package before using it. |
| Other backends | Microsoft RDP ActiveX/MsRdpEx and FreeRdpKit are not part of this wrapper. |

The package's native behavior depends on the shipped or custom executable, server configuration, gateway configuration, and Windows environment. This table is an implementation scope statement, not a certification of every OS, architecture, or authentication combination.

For Microsoft RDP hosting, see [RoyalApps Community RDP](https://github.com/royalapplications/royalapps-community-rdp).

