using ActualLab.Fusion.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using SpotifyVoiceCommander.Maui.Shared.Api;
using SpotifyVoiceCommander.Maui.Shared.Framework;
using SpotifyVoiceCommander.Maui.Shared.Lib.Etai;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.Shared;

internal static class Configure
{
    public static IServiceCollection ConfigureSharedLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly hostAssembly,
        Assembly[] additionalAssemblies)
    {
        var fullTargetAssemblies = additionalAssemblies.Append(hostAssembly).ToArray();

        services.AddSingleton<BackendUrlMapper>();

        // API
        services
            .AddInternalApi()
            .AddSpotifyClientWrapper();

        // Authentication
        services
           .AddAuthorizationCore()
           .AddScoped<AuthenticationStateProvider, MauiAuthenticationStateProvider>()
           .AddScoped(sp => (MauiAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

        // BlazorCircuitContext
        services
            .AddScoped(c => new MauiBlazorCircuitContext(c))
            .AddTransient(c => (BlazorCircuitContext)c.GetRequiredService<MauiBlazorCircuitContext>())
            .AddTransient(c => c.GetRequiredService<MauiBlazorCircuitContext>().Dispatcher);

        // UI
        services
            .AddEtaiServicesWithConfiguration();

        // Other
        services.AddFluxorServices(hostAssembly, additionalAssemblies);

        return services;
    }
}
