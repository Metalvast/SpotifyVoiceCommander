namespace SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager;

internal static class Configure
{
    public static IServiceCollection AddBeepManager(this IServiceCollection services) =>
        services.AddSingleton<IBeepManager, Impl.BeepManager>();
}
