namespace SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

internal static class Configure
{
    public static IServiceCollection AddSpotifyClientWrapper(this IServiceCollection services) =>
        services
            .AddSingleton<SpotifyClientHttpMessageHandler>()
            .AddScoped<SpotifyClientWrapper>();
}
