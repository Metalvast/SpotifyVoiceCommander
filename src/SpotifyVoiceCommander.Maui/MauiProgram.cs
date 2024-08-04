using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using SpotifyVoiceCommander.Maui.App;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;
using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

namespace SpotifyVoiceCommander.Maui;

public static class MauiProgram
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

        // Content
        services.ConfigureAppLayer(
            configuration,
            MauiSettings.MainAssembly,
            MauiSettings.TargetAssemblies);

        return builder.Build();
    }

   
}
