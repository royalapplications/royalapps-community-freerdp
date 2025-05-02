using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The security configuration for the FreeRdp connection
/// </summary>
[TypeConverter(typeof(SecurityConfigurationTypeConverter))]
public class SecurityConfiguration : IValidatableObject
{
    /// <summary>
    /// RDP protocol security
    /// </summary>
    public bool RDP { get; set; }

    /// <summary>
    /// TLS protocol security
    /// </summary>
    public bool TLS { get; set; }

    /// <summary>
    /// NLA protocol security
    /// </summary>
    public bool NLA { get; set; } = true;

    /// <summary>
    /// NLA extended protocol security
    /// </summary>
    public bool Ext { get; set; }

    /// <summary>
    /// AAD protocol security
    /// </summary>
    public bool AAD { get; set; }

    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        if (!RDP && !TLS && !NLA && !Ext && !AAD)
            return string.Empty;

        var builder = new StringBuilder("/sec:");

        if (RDP)
            builder.Append("rdp:on,");
        if (TLS)
            builder.Append("tls:on,");
        if (NLA)
            builder.Append("nla:on,");
        if (Ext)
            builder.Append("ext:on,");
        if (AAD)
            builder.Append("aad:on,");

        return builder.ToString().TrimEnd(',');
    }
 }

