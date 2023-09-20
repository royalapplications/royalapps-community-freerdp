using Microsoft.Extensions.Logging;

namespace RoyalApps.Community.FreeRdp.WinForms.Logging;

internal static class DebugLoggerFactory
{
    public static ILogger Create()
    {
#if DEBUG
        return DebugLogger.Instance;
#else
        return Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
#endif
    }
}