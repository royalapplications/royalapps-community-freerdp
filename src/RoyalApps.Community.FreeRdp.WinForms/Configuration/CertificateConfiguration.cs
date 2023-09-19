using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The certificate configuration for the FreeRdp connection
/// </summary>
[TypeConverter(typeof(CertificateConfigurationTypeConverter))]
public class CertificateConfiguration : IValidatableObject
{
    /// <summary>
    /// Automatically abort connection if the certificate does not match, no user interaction.
    /// </summary>
    public bool Deny { get; set; }
    
    /// <summary>
    /// Ignore the certificate checks altogether (overrules all other options).
    /// </summary>
    public bool Ignore { get; set; }

    /// <summary>
    /// Use the alternate name instead of the certificate subject to match locally stored certificates.
    /// </summary>
    public bool Name { get; set; }

    /// <summary>
    /// Ignore the certificate checks altogether (overrules all other options) locally stored certificates.
    /// </summary>
    public string? AlternateName { get; set; }

    /// <summary>
    /// Trust on first use: Accept certificate unconditionally on first connect and deny on subsequent connections if the certificate does not match.
    /// </summary>
    public bool TOFU { get; set; }

    /// <summary>
    /// Additional cache arguments. Do not start with a comma character!
    /// Example: codec[:rfx|nsc],persist,persist-file:filename]
    /// </summary>
    public string? AdditionalArguments { get; set; }
    
    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Name && string.IsNullOrEmpty(AlternateName))
            yield return new ValidationResult("An alternate name must be specified", new[] {nameof(AlternateName)});
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(AdditionalArguments) && !Deny && !Ignore && !Name && !TOFU)
            return string.Empty;
        
        var builder = new StringBuilder("/cert:");
        if (Deny)
            builder.Append("deny,");
        if (Ignore)
            builder.Append("ignore,");
        if (TOFU)
            builder.Append("tofu,");
        if (Name)
            builder.Append($"name:\"{AlternateName}\",");

        if (!string.IsNullOrEmpty(AdditionalArguments))
            builder.Append($"{AdditionalArguments}");

        return builder.ToString().TrimEnd(',');
    }
 }