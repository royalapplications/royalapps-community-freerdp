# FreeRDP Control

[![NuGet Version](https://img.shields.io/nuget/v/RoyalApps.Community.FreeRdp.WinForms.svg?style=flat)](https://www.nuget.org/packages/RoyalApps.Community.FreeRdp.WinForms) 
[![NuGet Downloads](https://img.shields.io/nuget/dt/RoyalApps.Community.FreeRdp.WinForms.svg?color=green)](https://www.nuget.org/packages/RoyalApps.Community.FreeRdp.WinForms) 
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-%3E%3D%204.72-512bd4)](https://dotnet.microsoft.com/download)
[![.NET](https://img.shields.io/badge/.NET-%3E%3D%20%205.0-blueviolet)](https://dotnet.microsoft.com/download)

RoyalApps.Community.FreeRDP contains projects/packages to easily embed/use [FreeRDP](https://github.com/FreeRDP/FreeRDP) in a Windows (WinForms) application.
![Screenshot](https://raw.githubusercontent.com/royalapplications/royalapps-community-freerdp/main/docs/assets/Screenshot.png)

The FreeRDP control starts the executable [`wfreerdp.exe`](https://github.com/FreeRDP/FreeRDP) and passes on the correct `parent-window` handle in order to render the remote desktop session in an embeddable WinForms control. The executable is shipped with the control (as embedded resource) and will be "extracted" to a configurable path (default is %temp%) before it is executed.

## Getting Started
### Installation
You should install the RoyalApps.Community.FreeRDP.WinForms with NuGet:
```
Install-Package RoyalApps.Community.FreeRDP.WinForms
```
or via the command line interface:
```
dotnet add package RoyalApps.Community.FreeRDP.WinForms
```

### Using the FreeRdpControl
#### Add Control
Place the `FreeRdpControl` on a form or in a container control (user control, tab control, etc.) and set the `Dock` property to `DockStyle.Fill`

#### Set Properties
To configure all RDP relevant settings, use the properties of the `FreeRdpConfiguration` class which is accessible through the `FreeRdpControl.Configuration` property.

#### Connect and Disconnect
Once the configuration is set, call:
```csharp
FreeRdpControl.Connect();
```
to start a connection.

> **Note**  
> Before you call `Connect();`, make sure you have set the `Server` (hostname or IP address) and the credential properties (`Username` and `Password`). An exception will be thrown if these properties are not set. If you connect to a Windows machine using the IP address, the connection may fail because the subject name of the certificate doesn't match. In this case, set `IgnoreCertificates` to `true`.

To disconnect, simply call:
```csharp
FreeRdpControl.Disconnect();
```
#### Zoom
Before you call `Connect();`, you can set the remote zoom level (DPI) using the following properties: 
`int DesktopScaleFactor` (valid values between 100 and 500)  
`int DeviceScaleFactor` (valid values: 100, 140, 180)  
> **Note** 
> Recommended values are for DeviceScaleFactor  
> 100 for DesktopScaleFactor of 100  
> 140 for DesktopScaleFactor between 100 and 199  
> 180 for DesktopScaleFactor of 200 or more  

##### Auto Scaling
If `FreeRdpConfiguration.AutoScaling` is set to `true`, the initial `DesktopScaleFactor` is determined based on DPI settings.

##### Changing Zoom Level
While connected you can use the following methods to change the remote zoom level:  
`ZoomIn()`, `ZoomOut()`, `ResetZoom()` and `SetZoomLevel(int scalingInPercent)`

> **Note**
> Calling these methods will kill the `wfreerdp.exe` and restart it using the new scaling values.

#### Subscribe to Events
When the connection has been established, the `Connected` event is raised.  

The `Disconnected` event is raised when:
* the connection couldn't be established (server not reachable, incorrect credentials)
* the connection has been interrupted (network failure)
* the connection was closed by the user (logoff or disconnect)
* the `wfreerdp.exe` died for some reason

The `DisconnectedEventArgs` may have an error code or error message for more information.

The `CertificateError` event is raised when the TLS handshake failed. Calling `e.Continue();` in the event handler will set the `FreeRdpConfiguration.IgnoreCertificate` property to true and retries the connection.

The `VerifyCredentials` event is raised when an authentication error occurs and the login fails. Calling `e.SetCredentials(string? username, string? domain, string? password);` in the event handler will set the credential properties in the `FreeRdpConfiguration` class and retries the connection with the provided credentials.

## Exploring the Demo Application
The demo application is quite simple. The `Connection` menu has the following items:
### Connect
Starts the remote desktop connection. 
If you click `Connect` the first time, you get a prompt for the server name to connect to and the a prompt for the credentials. If you want to change the server or the credentials, use the `Settings` window. 

### Disconnect
Stops the remote desktop connection by killing the `wfreerdp.exe` process associated with the session.

### Settings
Shows a window with all the settings from the `FreeRdpConfiguration` class. Edit/change the settings before you click on `Connect`. 

## Notable Features

### Auto Expand Desktop Size
If `DesktopWidth` and `DesktopHeight` properties are set to `0` (default), the remote desktop size is determined by the container size the control is placed on.  

### Smart Reconnect
If `SmartReconnect` is set to `true` and the container size has changed, the connection will automatically be closed and re-opened to adapt to the new desktop size.

## Acknowledgements
Special thanks to [Marc-André Moreau](https://github.com/awakecoding) and [akallabeth](https://github.com/akallabeth) for all the help.
