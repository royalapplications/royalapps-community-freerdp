using System.IO;
using System.Linq;
using System.Reflection;

namespace RoyalApps.Community.FreeRdp.WinForms.Extensions;

internal static class AssemblyExtensions
{
    public static byte[] GetResourceFileAsBytes(this Assembly assembly, string resourceName)
    {
        using var stream = GetResourceStream(assembly, resourceName);
        if (stream == null)
            return new byte[0];
        using var streamReader = new StreamReader(stream);
        using var memoryStream = new MemoryStream();
        streamReader.BaseStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    private static Stream? GetResourceStream(Assembly assembly, string resourceName)
    {
        string? name;
        if (resourceName.EndsWith("*"))
        {
            resourceName = resourceName.TrimEnd('*');
            name = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.ToLowerInvariant()
                    .Contains(resourceName.ToLowerInvariant()));
        }
        else
        {
            name = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.ToLowerInvariant()
                    .EndsWith(resourceName.ToLowerInvariant()));
        }
            
        return string.IsNullOrEmpty(name) ? null : assembly.GetManifestResourceStream(name);
    }
}