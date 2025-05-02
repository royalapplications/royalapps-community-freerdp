using System;
using System.ComponentModel;

namespace RoyalApps.Community.FreeRdp.WinForms.Configuration;

internal class SecurityConfigurationTypeConverter : TypeConverter
{
    public override bool GetPropertiesSupported(ITypeDescriptorContext? context)
    {
        return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object? value, Attribute[]? attributes)
    {
        return TypeDescriptor.GetProperties(typeof(SecurityConfiguration));
    }

    /// <summary>
    /// Overridden so that serialization still works - don't allow string serialization in the converter
    /// which allows JSON.NET to use its standard serialization. This also still works for the
    /// WinForms property sheet.
    /// </summary>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        if (destinationType == null)
            return false;

        if (destinationType == typeof(string))
            return true;

        return base.CanConvertTo(context, destinationType);
    }
}
