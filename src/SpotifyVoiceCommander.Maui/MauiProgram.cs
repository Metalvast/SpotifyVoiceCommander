using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Maui.Audio;
using SpotifyVoiceCommander.Maui.App;
using SpotifyVoiceCommander.Maui.App.ServicesAbstractions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;
using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

namespace SpotifyVoiceCommander.Maui;

public static partial class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
#if SHOW_BLAZOR_RENDER_INFO
        Etai.Blazor.EtaiBlazorConfigurator.SetShowRenderInfoState(true);
#endif
        MauiDiagnostics.Init();

        var builder = MauiApp
            .CreateBuilder()
            .UseMauiApp<MauiSharedApp>()
            .ConfigureLifecycleEvents(ConfigurePlatformLifecycleEvents)
            .AddEtaiMauiBlazorConfiguration(MauiSettings.BaseUrl, MauiSettings.Environment)
            .UseMauiCommunityToolkit()
            .AddAudio();

        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddTransient(c => new MauiRootPage());

        // Blazor
        services.AddMauiBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();
        services.AddScoped<Mutable<MauiWebView?>>();

        // Maui
        AddPlatformServices(services);

        // Content
        services.ConfigureAppLayer(
            configuration,
            MauiSettings.MainAssembly,
            MauiSettings.TargetAssemblies);

        var app = builder.Build();

        _ = app.Services.GetRequiredService<AppNonScopedServiceStarter>().StartNonScopedServices();

        return app;
    }

    private static partial void AddPlatformServices(this IServiceCollection services);
    private static partial void ConfigurePlatformLifecycleEvents(ILifecycleBuilder events);
}
