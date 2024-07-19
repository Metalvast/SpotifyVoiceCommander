using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using Serilog;
using SpotifyVoiceCommander.Maui.App;
using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

namespace SpotifyVoiceCommander.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
#if SHOW_BLAZOR_RENDER_INFO
        Etai.Blazor.EtaiBlazorConfigurator.SetShowRenderInfoState(true);
#endif
        var builder = MauiApp
            .CreateBuilder()
            .AddEtaiMauiBlazorConfiguration(MauiSettings.BaseUrl, MauiSettings.Environment)
            .UseMauiCommunityToolkit()
            .AddAudio()
            .UseMauiApp<MauiSharedApp>();

        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddTransient(c => new MauiRootPage());

        // Blazor
        services.AddMauiBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddScoped<Mutable<MauiWebView?>>();

        // System
        services.AddScoped(c => new ScopedServicesDisposeTracker(c));
        services.AddSingleton<MauiReloadUI>();
        services.AddMauiDiagnostics(false);

        // Content
        services.ConfigureAppLayer(
            configuration,
            MauiSettings.MainAssembly,
            MauiSettings.TargetAssemblies);

        return builder.Build();
    }

    private static IServiceCollection AddMauiDiagnostics(this IServiceCollection services, bool dispose) =>
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging
                .AddFilter(null, LogLevel.Information)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            logging.AddFilteringSerilog(Log.Logger, dispose: dispose);
        });
}
