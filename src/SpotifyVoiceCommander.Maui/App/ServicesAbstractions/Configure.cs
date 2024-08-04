namespace SpotifyVoiceCommander.Maui.App.ServicesAbstractions;

internal static class Configure
{
    public static IServiceCollection AddServicesAbstractions(this IServiceCollection services) =>
        services
            .AddSingleton<AppNonScopedServiceStarter>()
            .AddScoped<AppScopedServiceStarter>()
            .AddScoped(c => new ScopedServicesDisposeTracker(c));
}
