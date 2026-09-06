# Configuration Types

Namespace: `RoyalApps.Community.FreeRdp.WinForms.Configuration`

This page is a configuration overview. See the [generated FreeRdpConfiguration reference](/api/reference/royalapps-community-freerdp-winforms-configuration-freerdpconfiguration) and the namespace sidebar for every public configuration type and enum.

## FreeRdpConfiguration

The main configuration groups are:

| Group | Properties |
| --- | --- |
| Destination | `Server`, `Port`, `Username`, `Domain`, `Password` |
| Routing | `Gateway`, `Proxy`, `LoadBalanceInfo`, `PCB`, `VMId` |
| Display | `DesktopWidth`, `DesktopHeight`, `AutoScaling`, `DesktopScaleFactor`, `DeviceScaleFactor`, `SmartReconnect`, `ColorDepth` |
| Redirection | `Clipboard`, `AudioRedirection` |
| Security | `Certificate`, `Security`, `RestrictedAdminMode`, `ProtocolSecurityNegotiation` |
| Client process | `Executable`, `TempPath`, `AdditionalArguments` |
| Native reconnect | `AutoReconnect`, `AutoReconnectMaxRetries` |

`ParentWindow` is assigned by the control when it creates the render target; hosts normally do not set it.

Native `AutoReconnect` is distinct from wrapper-level `SmartReconnect`: the latter replaces the process after a size change.

[FreeRdpConfiguration source](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/FreeRdpConfiguration.cs) contains the complete property list, defaults, validation attributes, and argument mappings.

## GatewayConfiguration

`Hostname`, `Port`, `Username`, `Domain`, `Password`, and `AdditionalArguments` describe the RD Gateway independently of the destination.

An empty hostname suppresses gateway arguments. Password and additional arguments can contain secrets; do not log `ToString()`.

[GatewayConfiguration source](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/GatewayConfiguration.cs).

## CertificateConfiguration

`Deny`, `Ignore`, `Name`, `AlternateName`, `TOFU`, and `AdditionalArguments` map to native certificate options. `Ignore` bypasses checks; `TOFU` means trust on first use and has its own security tradeoffs.

[CertificateConfiguration source](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/CertificateConfiguration.cs).

## Other nested settings

- [ProxyConfiguration](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/ProxyConfiguration.cs)
- [SecurityConfiguration](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/SecurityConfiguration.cs)
- [CacheConfiguration](https://github.com/royalapplications/royalapps-community-freerdp/blob/main/src/RoyalApps.Community.FreeRdp.WinForms/Configuration/CacheConfiguration.cs)

See the [Configuration guide](/articles/configuration) for examples.
