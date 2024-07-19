using Microsoft.Extensions.Configuration;
using SpotifyVoiceCommander.Maui.Shared;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.App;

internal static class Configure
{
    public static IServiceCollection ConfigureAppLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly hostAssembly,
        Assembly[] additionalAssemblies)
    {
        services.ConfigureSharedLayer(
            configuration,
            hostAssembly,
            additionalAssemblies);

        return services;
    }
}
