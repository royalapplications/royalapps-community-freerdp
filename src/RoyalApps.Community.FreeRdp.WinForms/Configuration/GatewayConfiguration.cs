using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

/// <summary>
/// The gateway configuration for the FreeRdp connection
/// </summary>
[TypeConverter(typeof(GatewayConfigurationTypeConverter))]
public class GatewayConfiguration : IValidatableObject
{
    /// <summary>
    /// Hostname of the gateway, If this property is null (or empty), all gateway settings are ignored 
    /// </summary>
    public string? Hostname { get; set; }
    
    /// <summary>
    /// Port number the gateway listens to
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// Gateway username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gateway user domain
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// Password of the gateway user
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Additional gateway arguments. Do not start with a comma character!
    /// Example: usage-method:[direct|detect],access-token:token,type:[rpc|http[,no-websockets][,extauth-sspi-ntlm]|auto[,no-websockets][,extauth-sspi-ntlm]]|arm,url:wss://url,bearer:oauth2-bearer-token
    /// </summary>
    public string? AdditionalArguments { get; set; }
    
    /// <inheritdoc cref="IValidatableObject"/>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Hostname)) 
            yield break;
        
        if (Port is < 1 or > 65535)
            yield return new ValidationResult("The proxy port number is invalid",
                new[] {nameof(Port)});
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        // /g:gateway[:port],u:user,d:domain,p:password[,additionalargs],

        if (string.IsNullOrEmpty(Hostname))
            return string.Empty;
        
        var builder = new StringBuilder($"/gateway:g:{Hostname}");

        if (Port is not null)
        {
            builder.Append(":");
            builder.Append(Port);
        }

        if (!string.IsNullOrEmpty(Username))
            builder.Append($",u:{Username}");

        if (!string.IsNullOrEmpty(Domain))
            builder.Append($",d:{Domain}");

        if (!string.IsNullOrEmpty(Password))
            builder.Append($"\",p:{Password}\"");

        if (!string.IsNullOrEmpty(AdditionalArguments))
            builder.Append($",{AdditionalArguments}");

        return builder.ToString();
    }
 }