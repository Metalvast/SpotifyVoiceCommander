using MudBlazor.Services;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Etai;

internal static class Configure
{
    public static IServiceCollection AddEtaiServicesWithConfiguration(this IServiceCollection services) =>
        services
            .AddServicesInitializer()
            .AddMudServices()
            .AddEtaiBlazor()
            .AddScoped<ICurrentWebViewContextProvider, MauiCurrentWebViewContextProvider>();
}
