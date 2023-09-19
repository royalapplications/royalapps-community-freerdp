using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The cache configuration for the FreeRdp connection
/// </summary>
[TypeConverter(typeof(CacheConfigurationTypeConverter))]
public class CacheConfiguration : IValidatableObject
{
    /// <summary>
    /// Bitmap caching 
    /// </summary>
    public bool Bitmap { get; set; }
    
    /// <summary>
    /// Glyph caching
    /// </summary>
    public bool Glyph { get; set; }

    /// <summary>
    /// Off-screen caching
    /// </summary>
    public bool Offscreen { get; set; }

    /// <summary>
    /// Additional cache arguments. Do not start with a comma character!
    /// Example: codec[:rfx|nsc],persist,persist-file:filename]
    /// </summary>
    public string? AdditionalArguments { get; set; }
    
    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield break;
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(AdditionalArguments) && !Bitmap && !Glyph && !Offscreen)
            return string.Empty;
        
        var builder = new StringBuilder($"/cache:");
        builder.Append($"bitmap:{(Bitmap ? "on" : "off")}");
        builder.Append($"glyph:{(Glyph ? "on" : "off")}");
        builder.Append($"offscreen:{(Offscreen ? "on" : "off")}");

        if (!string.IsNullOrEmpty(AdditionalArguments))
            builder.Append($",{AdditionalArguments}");

        return builder.ToString();
    }
 }