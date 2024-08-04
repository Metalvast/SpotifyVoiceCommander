using ActualLab.Fusion.Blazor;
using Microsoft.Extensions.Logging;
using Serilog;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

internal static class Configure
{
    public static IServiceCollection AddMauiServices(this IServiceCollection services) =>
        services
            .AddMauiAppRestarter()
            .AddMauiDiagnostics(false)
            .AddMauiCircuitContext()
            .AddSingleton<MauiReloadUI>()
            .AddSingleton<MauiRecognizerStarterService>();

    public static IServiceCollection AddMauiDiagnostics(
        this IServiceCollection services,
        bool dispose) =>
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging
                .AddFilter(null, LogLevel.Information)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("SpotifyVoiceCommander", LogLevel.Debug);
            logging.AddFilteringSerilog(Log.Logger, dispose: dispose);
        });

    public static IServiceCollection AddMauiCircuitContext(this IServiceCollection services) =>
        services
            .AddScoped(c => new MauiBlazorCircuitContext(c))
            .AddTransient(c => (BlazorCircuitContext)c.GetRequiredService<MauiBlazorCircuitContext>())
            .AddTransient(c => c.GetRequiredService<MauiBlazorCircuitContext>().Dispatcher);
}
