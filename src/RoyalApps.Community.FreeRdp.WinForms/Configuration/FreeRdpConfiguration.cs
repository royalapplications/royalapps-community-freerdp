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
    #region --- CLI ---

    /// <summary>
    /// AdditionalArguments: Specify one or more additional arguments when wfreerdp.exe is called
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Admin (or console) session
    /// </summary>
    [CommandLineArgument("/admin")]
    public bool Admin { get; set; }

    /// <summary>
    /// Desktop composition
    /// </summary>
    [CommandLineToggleArgument("aero", false)]
    public bool Aero { get; set; }

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
    public AudioRedirectionMode AudioRedirection { get; set; } = AudioRedirectionMode.NotSpecified;

    /// <summary>
    /// Automatic reconnection
    /// </summary>
    [CommandLineArgument("/auto-reconnect")]
    public bool AutoReconnect { get; set; }

    /// <summary>
    /// Automatic reconnection maximum retries, 0 for unlimited [0, 1000]
    /// </summary>
    [CommandLineArgument("/auto-reconnect-max-retries:{0}", 0)]
    public int? AutoReconnectMaxRetries { get; set; }

    /// <summary>
    /// Session bpp (color depth)
    /// </summary>
    /// <remarks>
    /// Command line argument: /bpp:[8|16|24|32]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpclient-colordepth">ColorDepth - Microsoft Documentation</see>
    [CommandLineArgument("/bpp:{0}", BitsPerPixel.NotSpecified)]
    public BitsPerPixel ColorDepth { get; set; } = BitsPerPixel.NotSpecified;

    /// <summary>
    /// Cache configuration
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(CacheConfigurationTypeConverter))]
    public CacheConfiguration Cache { get; set; } = new();

    /// <summary>
    /// Certificate configuration
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(CertificateConfigurationTypeConverter))]
    public CertificateConfiguration Certificate { get; set; } = new();

    /// <summary>
    /// RedirectClipboard (default on): -clipboard
    /// </summary>
    [CommandLineToggleArgument("clipboard", true)]
    public bool Clipboard { get; set; } = true;

    /// <summary>
    /// Compression (default on): -compression
    /// </summary>
    [CommandLineToggleArgument("compression", true)]
    public bool Compression { get; set; } = true;

    /// <summary>
    /// Domain: /d:Domain
    /// </summary>
    [CommandLineArgument("/d:{0}", "")]
    public string? Domain { get; set; }

    /// <summary>
    /// SmoothFonts (default on): -fonts
    /// </summary>
    [CommandLineToggleArgument("fonts", true)]
    public bool Fonts { get; set; } = true;

    /// <summary>
    /// Gateway configuration
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(GatewayConfigurationTypeConverter))]
    public GatewayConfiguration Gateway { get; set; } = new();

    /// <summary>
    /// GDI rendering mode.
    /// </summary>
    /// <remarks>
    /// Command line argument: /gdi:[sw|hw]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpextendedsettings-property">EnableHardwareMode - Microsoft Documentation</see>
    [CommandLineArgument("/gdi:{0}", GDIRendering.NotSpecified)]
    public GDIRendering GDI { get; set; } = GDIRendering.NotSpecified;

    /// <summary>
    /// DesktopHeight (pixel): /h:DesktopHeight
    /// </summary>
    [CommandLineArgument("/h:{0}", 0)]
    public int DesktopHeight { get; set; }

    /// <summary>
    /// KeyboardLayout (0xlayout-id or layout name): /kdb:layout:[0xlayout-id]|[layout-name]
    /// </summary>
    [CommandLineArgument("/kbd:layout:{0}")]
    public string? KeyboardLayout { get; set; }

    /// <summary>
    /// Load balance info
    /// </summary>
    [CommandLineArgument("/load-balance-info:{0}")]
    public string? LoadBalanceInfo { get; set; }

    /// <summary>
    /// MenuAnimations (default: off): +menu-anims
    /// </summary>
    [CommandLineToggleArgument("menu-anims", false)]
    public bool MenuAnimations { get; set; }

    /// <summary>
    /// ProtocolSecurityNegotiation (default on): -nego
    /// </summary>
    [CommandLineToggleArgument("nego", true)]
    public bool ProtocolSecurityNegotiation { get; set; } = true;

    /// <summary>
    /// Network connection type is not specified. The default value of wfreerdp.exe is used.
    /// </summary>
    /// <remarks>
    /// Command line argument: /network:[modem|broadband[-low|-high]|wan|lan|auto[detect]]
    /// </remarks>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/termserv/imsrdpclientadvancedsettings7-networkconnectiontype">NetworkConnectionType - Microsoft Documentation</see>
    [CommandLineArgument("/network:{0}", NetworkConnectionType.NotSpecified)]
    public NetworkConnectionType Network { get; set; } = NetworkConnectionType.NotSpecified;

    /// <summary>
    /// Password: /p:Password
    /// </summary>
    [CommandLineArgument("/p:\"{0}\"", "")]
    public string? Password { get; set; }

    /// <summary>
    /// ParentWindow: /parent-window:ParentWindow
    /// </summary>
    [CommandLineArgument("/parent-window:{0}", 0)]
    public long ParentWindow { get; set; }

    /// <summary>
    /// PCB (Preconnection Blob): /pcb:PCB
    /// </summary>
    [CommandLineArgument("/pcb:{0}")]
    public string? PCB { get; set; }

    /// <summary>
    /// Port: /port:Port
    /// </summary>
    [CommandLineArgument("/port:{0}", 3389)]
    [Range(0, 65535)]
    public int Port { get; set; } = 3389;

    /// <summary>
    /// ProxyConfiguration /proxy:[http|socks5]://[username]:[password]@hostname:port
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(ProxyConfigurationTypeConverter))]
    public ProxyConfiguration Proxy { get; set; } = new();

    /// <summary>
    /// Restricted admin mode
    /// </summary>
    [CommandLineArgument("/restricted-admin")]
    public bool RestrictedAdminMode { get; set; }

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
    /// DesktopScaleFactor in percent (100-500): /scale-desktop:DesktopScaleFactor
    /// </summary>
    [CommandLineArgument("/scale-desktop:{0}", 100)]
    [Range(100, 500)]
    public int DesktopScaleFactor { get; set; } = 100;

    /// <summary>
    /// ProxyConfiguration /proxy:[http|socks5]://[username]:[password]@hostname:port
    /// </summary>
    [Required]
    [CommandLineArgument("{0}")]
    [TypeConverter(typeof(SecurityConfigurationTypeConverter))]
    public SecurityConfiguration Security { get; set; } = new();

    /// <summary>
    /// StartProgram (Alternate shell): /shell:StartProgram
    /// </summary>
    [CommandLineArgument("/shell:\"{0}\"")]
    public string? Shell { get; set; }

    /// <summary>
    /// WorkDir (Shell working directory): /shell-dir:WorkDir
    /// </summary>
    [CommandLineArgument("/shell-dir:\"{0}\"")]
    public string? ShellDir { get; set; }

    /// <summary>
    /// Themes (default: on): -themes
    /// </summary>
    [CommandLineToggleArgument("themes", true)]
    public bool Themes { get; set; } = true;

    /// <summary>
    /// Username: /u:Username
    /// </summary>
    [CommandLineArgument("/u:{0}", "")]
    public string? Username { get; set; }

    /// <summary>
    /// Server hostname: /v:Server
    /// </summary>
    [CommandLineArgument("/v:{0}")]
    [Required]
    public string Server { get; set; } = null!;

    /// <summary>
    /// VMId (use port 2179, disable negotiation): /vmconnect:VMId
    /// </summary>
    [CommandLineArgument("/vmconnect:{0}")]
    public string? VMId { get; set; }

    /// <summary>
    /// DesktopWidth (pixel): /w:DesktopWidth
    /// </summary>
    [CommandLineArgument("/w:{0}", 0)]
    public int DesktopWidth { get; set; }

    /// <summary>
    /// Wallpaper (default: on): -wallpaper
    /// </summary>
    [CommandLineToggleArgument("wallpaper", true)]
    public bool Wallpaper { get; set; } = true;

    /// <summary>
    /// Window Drag (default off): +window-drag
    /// </summary>
    [CommandLineToggleArgument("window-drag", false)]
    public bool WindowDrag { get; set; }

    #endregion

    #region --- Control Settings ---

    /// <summary>
    /// AutoScaling: When enabled, the initial scale factor is determined based on DPI settings
    /// </summary>
    public bool AutoScaling { get; set; } = true;

    /// <summary>
    /// SmartReconnect: When enabled, the connection will be re-established to adapt to the new desktop size
    /// </summary>
    public bool SmartReconnect { get; set; }

    #endregion

    #region --- Executable ---

    /// <summary>
    /// The full path to an alternative wfreerdp.exe
    /// </summary>
    public string? Executable { get; set; }

    /// <summary>
    /// TempPath: The directory where wfreerdp.exe is written to if not already available
    /// </summary>
    public string TempPath { get; set; } = "%temp%";

    #endregion

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
        Validator.TryValidateObject(Proxy, new ValidationContext(Proxy), errors, true);
        Validator.TryValidateObject(Gateway, new ValidationContext(Gateway), errors, true);

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
                                yield return string.Format(commandArgumentAttribute.ArgumentFormat, (int) propertyValue).Trim();
                            }
                            else
                            {
                                // use description
                                yield return string.Format(commandArgumentAttribute.ArgumentFormat, descriptionAttribute.Description).Trim();
                            }
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            if ((bool) propertyValue)
                                yield return commandArgumentAttribute.ArgumentFormat.Trim();
                        }
                        else
                        {
                            yield return string.Format(commandArgumentAttribute.ArgumentFormat, propertyValue).Trim();
                        }
                    }
                        break;
                    case CommandLineToggleArgumentAttribute commandToggleArgumentAttribute:
                    {
                        var defaultValue = commandToggleArgumentAttribute.DefaultValue;
                        if (propertyValue == null || propertyValue.Equals(defaultValue))
                            continue;

                        yield return ((bool)propertyValue ? "+" : "-") + commandToggleArgumentAttribute.ToggleText.Trim();
                    }
                        break;
                }
            }
        }

        if (AdditionalArguments != null && !string.IsNullOrWhiteSpace(AdditionalArguments))
            yield return AdditionalArguments.Trim();
    }
}
