using ActualLab.Fusion.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using SpotifyVoiceCommander.Maui.Shared.Api;
using SpotifyVoiceCommander.Maui.Shared.Api.Lib;
using SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.AuthenticationStateProvider;
using SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.Etai;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.Shared;

internal static class Configure
{
    public static IServiceCollection ConfigureSharedLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly hostAssembly,
        Assembly[] additionalAssemblies) =>
        services
            // Api
            .AddInternalApi()
            .AddSpotifyClientWrapper()
            // Blazor
            .AddSvcAuthenticationStateProvider()
            .AddSvcNavigationManager()
            // Maui
            .AddMauiServices()
            // Audio services
            .AddBeepManager()
            .AddAdditionalAudioServices()
            // Store
            .AddFluxorServices(hostAssembly, additionalAssemblies)
            // UI
            .AddEtaiServicesWithConfiguration();
}
