using System;

namespace RoyalApps.Community.FreeRdp.WinForms.Attributes;

internal class CommandLineToggleArgumentAttribute : Attribute
{
    public string ToggleText { get; }
    public bool DefaultValue { get; }

    public CommandLineToggleArgumentAttribute(string toggleText)
    {
        ToggleText = toggleText;
    }

    public CommandLineToggleArgumentAttribute(string toggleText, bool defaultValue)
    {
        ToggleText = toggleText;
        DefaultValue = defaultValue;
    }

}