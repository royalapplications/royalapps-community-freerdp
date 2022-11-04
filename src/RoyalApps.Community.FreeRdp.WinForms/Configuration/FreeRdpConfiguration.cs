using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using RoyalApps.Community.FreeRdp.WinForms.Attributes;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// Encapsulates configuration for <see cref="FreeRdpControl"/>.
/// </summary>
[TypeConverter(typeof(FreeRdpConfigurationTypeConverter))]
public class FreeRdpConfiguration : IValidatableObject
{
    /// <summary>
    /// Server hostname: /v:Server
    /// </summary>
    [CommandLineArgument("/v:{0}")]
    [Required]
    public string Server { get; set; } = null!;

    /// <summary>
    /// Port: /port:Port
    /// </summary>
    [CommandLineArgument("/port:{0}", 3389)]
    [Range(0, 65535)]
    public int Port { get; set; } = 3389;

    /// <summary>
    /// UserName: /u:UserName
    /// </summary>
    [CommandLineArgument("/u:{0}", "")]
    public string? UserName { get; set; }

    /// <summary>
    /// Domain: /d:Domain
    /// </summary>
    [CommandLineArgument("/d:{0}", "")]
    public string? Domain { get; set; }

    /// <summary>
    /// Password: /p:Password
    /// </summary>
    [CommandLineArgument("/p:\"{0}\"", "")]
    public string? Password { get; set; }

    /// <summary>
    /// NetworkLevelAuthentication (NLA) default on, turn off with: -sec-nla
    /// </summary>
    [CommandLineToggleArgument("sec-nla", true)]
    public bool NetworkLevelAuthentication { get; set; } = true;

    /// <summary>
    /// The color depth (in bits per pixel) for the control's connection.
    /// </summary>
    /// <remarks>
    /// Command line argument: /bpp:[8|16|24|32]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpclient-colordepth">ColorDepth - Microsoft Documentation</see>
    [CommandLineArgument("/bpp:{0}", BitsPerPixel.NotSpecified)]
    public BitsPerPixel ColorDepth { get; set; } = BitsPerPixel.NotSpecified;

    /// <summary>
    /// DesktopWidth (pixel): /w:DesktopWidth
    /// </summary>
    [CommandLineArgument("/w:{0}", 0)]
    public int DesktopWidth { get; set; }

    /// <summary>
    /// DesktopHeight (pixel): /h:DesktopHeight
    /// </summary>
    [CommandLineArgument("/h:{0}", 0)]
    public int DesktopHeight { get; set; }

    /// <summary>
    /// DeviceScaleFactor (100, 140, 180): /scale:DeviceScaleFactor
    /// Recommended values:
    /// 100: for DesktopScaleFactor of 100
    /// 140: for DesktopScaleFactor between 100 and 199
    /// 180: for DesktopScaleFactor of 200 or more
    /// </summary>
    [CommandLineArgument("/scale:{0}", 100)]
    public int DeviceScaleFactor { get; set; } = 100;

    /// <summary>
    /// AutoScaling: When enabled, the initial scale factor is determined based on DPI settings 
    /// </summary>
    public bool AutoScaling { get; set; }

    /// <summary>
    /// DesktopScaleFactor in percent (100-500): /scale-desktop:DesktopScaleFactor
    /// </summary>
    [CommandLineArgument("/scale-desktop:{0}", 100)]
    [Range(100, 500)]
    public int DesktopScaleFactor { get; set; } = 100;

    /// <summary>
    /// Compression (default off): +compression  
    /// </summary>
    [CommandLineToggleArgument("compression")]
    public bool Compression { get; set; }

    /// <summary>
    /// BitmapCaching (default on): -bitmap-cache 
    /// </summary>
    [CommandLineToggleArgument("bitmap-cache", true)]
    public bool BitmapCaching { get; set; } = true;

    /// <summary>
    /// ConnectToAdministerServer: /admin
    /// </summary>
    [CommandLineArgument("/admin")]
    public bool ConnectToAdministerServer { get; set; }

    /// <summary>
    /// KeyboardLayout (0xlayout-id or layout name): /kdb:[0xlayout-id]|[layout-name]
    /// </summary>
    [CommandLineArgument("/kbd:{0}")]
    public string? KeyboardLayout { get; set; }

    /// <summary>
    /// ProxyConfiguration /proxy:[http|socks5]://[username]:[password]@hostname:port
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(ProxyConfigurationTypeConverter))]
    public ProxyConfiguration ProxyConfiguration { get; set; } = new();
        
    /// <summary>
    /// GatewayHostname[:Port]: /g:GatewayHostname[:Port] 
    /// </summary>
    [CommandLineArgument("/g:{0}")]
    public string? GatewayHostname { get; set; }

    /// <summary>
    /// GatewayUserName: /gu:GatewayUserName
    /// </summary>
    [CommandLineArgument("/gu:{0}", "")]
    public string? GatewayUserName { get; set; }

    /// <summary>
    /// GatewayDomain: /gd:GatewayDomain
    /// </summary>
    [CommandLineArgument("/gd:{0}", "")]
    public string? GatewayDomain { get; set; }

    /// <summary>
    /// GatewayPassword: /gp:GatewayPassword
    /// </summary>
    [CommandLineArgument("/gp:\"{0}\"", "")]
    public string? GatewayPassword { get; set; }

    /// <summary>
    /// StartProgram (Alternate shell): /shell:StartProgram
    /// </summary>
    [CommandLineArgument("/shell:\"{0}\"")]
    public string? StartProgram { get; set; }

    /// <summary>
    /// WorkDir (Shell working directory): /shell-dir:WorkDir
    /// </summary>
    [CommandLineArgument("/shell-dir:\"{0}\"")]
    public string? WorkDir { get; set; }

    /// <summary>
    /// Sets and retrieves the audio redirection mode and different audio redirection options.
    /// </summary>
    /// <remarks>
    /// Command line argument: /audio-mode:[0|1|2]
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>0</term>
    /// <description>Redirect locally</description>
    /// </item>
    /// <item>
    /// <term>1</term>
    /// <description>Leave on server</description>
    /// </item>
    /// <item>
    /// <term>2</term>
    /// <description>None</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpclientadvancedsettings5-audioredirectionmode">AudioRedirectionMode - Microsoft Documentation</see>
    [CommandLineArgument("/audio-mode:{0}", AudioRedirectionMode.NotSpecified)]
    public AudioRedirectionMode AudioRedirectionMode { get; set; } = AudioRedirectionMode.NotSpecified;

    /// <summary>
    /// AudioCaptureRedirectionMode: /mic
    /// </summary>
    [CommandLineArgument("/mic")]
    public bool AudioCaptureRedirectionMode { get; set; }

    /// <summary>
    /// Network connection type is not specified. The default value of wfreerdp.exe is used.
    /// </summary>
    /// <remarks>
    /// Command line argument: /network:[modem|broadband[-low|-high]|wan|lan|auto[detect]]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpclientadvancedsettings7-networkconnectiontype">NetworkConnectionType - Microsoft Documentation</see>
    [CommandLineArgument("/network:{0}", NetworkConnectionType.NotSpecified)]
    public NetworkConnectionType NetworkConnectionType { get; set; } = NetworkConnectionType.NotSpecified;

    /// <summary>
    /// RedirectClipboard (default off): +clipboard 
    /// </summary>
    [CommandLineToggleArgument("clipboard")]
    public bool RedirectClipboard { get; set; }

    /// <summary>
    /// SmoothFonts (default off): +fonts
    /// </summary>
    [CommandLineToggleArgument("fonts")]
    public bool SmoothFonts { get; set; }

    /// <summary>
    /// Aero (Desktop Composition - default off): +aero
    /// </summary>
    [CommandLineToggleArgument("aero")]
    public bool Aero { get; set; }

    /// <summary>
    /// Window Drag (default off): +window-drag
    /// </summary>
    [CommandLineToggleArgument("window-drag")]
    public bool WindowDrag { get; set; }

    /// <summary>
    /// MenuAnimations (default: off): +menu-anims
    /// </summary>
    [CommandLineToggleArgument("menu-anims")]
    public bool MenuAnimations { get; set; }

    /// <summary>
    /// Themes (default: on): -themes
    /// </summary>
    [CommandLineToggleArgument("themes", true)]
    public bool Themes { get; set; } = true;

    /// <summary>
    /// Wallpaper (default: on): -wallpaper
    /// </summary>
    [CommandLineToggleArgument("wallpaper", true)]
    public bool Wallpaper { get; set; } = true;

    /// <summary>
    /// GDI rendering mode.
    /// </summary>
    /// <remarks>
    /// Command line argument: /gdi:[sw|hw]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpextendedsettings-property">EnableHardwareMode - Microsoft Documentation</see>
    [CommandLineArgument("/gdi:{0}", GdiRendering.NotSpecified)]
    public GdiRendering GdiRendering { get; set; } = GdiRendering.NotSpecified;

    /// <summary>
    /// IgnoreCertificate: /cert-ignore
    /// </summary>
    [CommandLineArgument("/cert-ignore")]
    public bool IgnoreCertificate { get; set; }

    /// <summary>
    /// PCB (Preconnection Blob): /pcb:PCB
    /// </summary>
    [CommandLineArgument("/pcb:{0}")]
    public string? PCB { get; set; }

    /// <summary>
    /// ProtocolSecurityNegotiation (default on): -nego
    /// </summary>
    [CommandLineToggleArgument("nego", true)]
    public bool ProtocolSecurityNegotiation { get; set; } = true;

    /// <summary>
    /// VMId (use port 2179, disable negotiation): /vmconnect:VMId
    /// </summary>
    [CommandLineArgument("/vmconnect:{0}")]
    public string? VMId { get; set; }

    /// <summary>
    /// ParentWindow: /parent-window:ParentWindow
    /// </summary>
    [CommandLineArgument("/parent-window:{0}", 0)]
    public long ParentWindow { get; set; }

    /// <summary>
    /// SmartReconnect: When enabled, the connection will be re-established to adapt to the new desktop size
    /// </summary>
    public bool SmartReconnect { get; set; }
        
    /// <summary>
    /// TempPath: The directory where wfreerdp.exe is written to if not already available
    /// </summary>
    public string TempPath { get; set; } = "%temp%";

    /// <summary>
    /// AdditionalArgs: Specify one or more additional arguments when wfreerdp.exe is called
    /// </summary>
    public string? AdditionalArgs { get; set; }

    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DeviceScaleFactor != 100 && DeviceScaleFactor != 140 && DeviceScaleFactor != 180)
            yield return new ValidationResult("Device scale factor must be 100, 140 or 180",
                new[] {nameof(DeviceScaleFactor)});
    }

    internal IEnumerable<string> GetArguments()
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(this, new ValidationContext(this), errors, true);
        Validator.TryValidateObject(ProxyConfiguration, new ValidationContext(ProxyConfiguration), errors, true);
            
        if (errors.Any())
            throw new ArgumentException(
                $"One or more errors occurred:{Environment.NewLine}{string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage))}");

        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            
        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                var propertyValue = property.GetValue(this);
                switch (attribute)
                {
                    case CommandLineArgumentAttribute commandArgumentAttribute:
                    {
                        var defaultValue = commandArgumentAttribute.DefaultValue;
                        if (propertyValue == null || propertyValue.Equals(defaultValue))
                            continue;

                        if (property.PropertyType.IsEnum)
                        {
                            var descriptionAttribute = property.PropertyType
                                .GetMember(propertyValue.ToString()!).First()
                                .GetCustomAttribute<DescriptionAttribute>();
                            if (descriptionAttribute == null)
                            {
                                // use int
                                yield return string.Format(commandArgumentAttribute.ArgumentFormat, (int) propertyValue);
                            }
                            else
                            {
                                // use description
                                yield return string.Format(commandArgumentAttribute.ArgumentFormat, descriptionAttribute.Description);
                            }
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            if ((bool) propertyValue)
                                yield return commandArgumentAttribute.ArgumentFormat;
                        }
                        else
                        {
                            yield return string.Format(commandArgumentAttribute.ArgumentFormat, propertyValue);
                        }
                    }
                        break;
                    case CommandLineToggleArgumentAttribute commandToggleArgumentAttribute:
                    {
                        var defaultValue = commandToggleArgumentAttribute.DefaultValue;
                        if (propertyValue == null || propertyValue.Equals(defaultValue))
                            continue;

                        yield return ((bool)propertyValue ? "+" : "-") + commandToggleArgumentAttribute.ToggleText;
                    }
                        break;
                }
            }
        }

        if (AdditionalArgs != null && !string.IsNullOrWhiteSpace(AdditionalArgs))
            yield return AdditionalArgs;
    }
}