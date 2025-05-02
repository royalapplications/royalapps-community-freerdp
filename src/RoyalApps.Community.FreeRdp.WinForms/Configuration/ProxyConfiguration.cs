using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The proxy configuration for the FreeRdp connection
/// </summary>
[TypeConverter(typeof(ProxyConfigurationTypeConverter))]
public class ProxyConfiguration : IValidatableObject
{
    /// <summary>
    /// The proxy mode for the configuration
    /// </summary>
    public ProxyMode ProxyMode { get; set; } = ProxyMode.None;

    /// <summary>
    /// The proxy host name to use
    /// </summary>
    public string? ProxyHost { get; set; }

    /// <summary>
    /// The proxy port
    /// </summary>
    public int ProxyPort { get; set; } = 8080;

    /// <summary>
    /// The proxy username
    /// </summary>
    public string? ProxyUsername { get; set; }

    /// <summary>
    /// The proxy password
    /// </summary>
    public string? ProxyPassword { get; set; }

    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ProxyMode != ProxyMode.None && string.IsNullOrWhiteSpace(ProxyHost))
            yield return new ValidationResult("A proxy host must be specified",
                new[] {nameof(ProxyHost)});
        if (ProxyMode != ProxyMode.None && ProxyPort is < 1 or > 65535)
            yield return new ValidationResult("The proxy port number is invalid",
                new[] {nameof(ProxyPort)});
        if (ProxyMode != ProxyMode.None && string.IsNullOrEmpty(ProxyUsername) && !string.IsNullOrEmpty(ProxyPassword))
            yield return new ValidationResult("Username is not set",
                new[] {nameof(ProxyUsername)});
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        if (ProxyMode == ProxyMode.None)
            return string.Empty;

        var builder = new StringBuilder("/proxy:");
        builder.Append(
            ProxyMode == ProxyMode.SOCKS5
                ? "socks5://"
                : "http://");

        if (!string.IsNullOrEmpty(ProxyUsername))
            builder.Append(ProxyUsername);

        if (!string.IsNullOrEmpty(ProxyUsername) &&
            !string.IsNullOrEmpty(ProxyPassword))
        {
            builder.Append(":");
            builder.Append(ProxyPassword);
        }

        if (!string.IsNullOrEmpty(ProxyUsername))
            builder.Append("@");

        builder.Append(ProxyHost);
        builder.Append(":");
        builder.Append(ProxyPort);

        return builder.ToString();
    }
 }

