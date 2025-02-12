using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyVoiceCommander.Maui.App.ServicesAbstractions;

[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ScopedServicesDisposeTracker))]
public sealed class ScopedServicesDisposeTracker(IServiceProvider services) : IDisposable
{
    private static ILogger? _log;
    private static ILogger Log => _log ??= MauiDiagnostics.LoggerFactory.CreateLogger<ScopedServicesDisposeTracker>();

    public void Dispose()
    {
        TryDiscardActiveScopedServices(services, $"{nameof(ScopedServicesDisposeTracker)}.{nameof(Dispose)}");
        Log.LogInformation("Dispose; stack trace:\n{StackTrace}", Environment.StackTrace);
    }
}
