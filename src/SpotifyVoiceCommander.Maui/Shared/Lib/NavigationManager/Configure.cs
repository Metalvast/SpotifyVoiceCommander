namespace SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

internal static class Configure
{
    public static IServiceCollection AddSvcNavigationManager(this IServiceCollection services) =>
        services.AddScoped<SvcNavigationManager>();
}
