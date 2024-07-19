using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Middlewares;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Queues;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor;

internal static class Configure
{
    public static IServiceCollection AddFluxorServices(
        this IServiceCollection services,
        Assembly hostAssembly,
        Assembly[] additionalAssemblies) =>
        services
            .AddFluxor(fluxorOptions =>
            {
                fluxorOptions.ScanAssemblies(hostAssembly, additionalAssemblies);
                fluxorOptions.AddMiddleware<AuthenticateStateCheckMiddleware>();
            })
            .AddScoped<AfterAuthenticationActionQueueDispatcher>()
            .AddTransient<SvcFluxorActionResolver>();
}
