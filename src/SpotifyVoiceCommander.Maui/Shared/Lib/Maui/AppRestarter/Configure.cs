using SpotifyVoiceCommander.Maui.Shared.Lib.AppRestarter;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;

internal static class Configure
{
    public static IServiceCollection AddMauiAppRestarter(this IServiceCollection services) =>
        services.AddSingleton<IMauiAppRestarter, MauiAppRestarter>();
}
