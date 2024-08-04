namespace SpotifyVoiceCommander.Maui.Shared.Lib.AuthenticationStateProvider;

internal static class Configure
{
    public static IServiceCollection AddSvcAuthenticationStateProvider(this IServiceCollection services) =>
        services
            .AddAuthorizationCore()
            .AddScoped<BlazorAuthenticationStateProvider, SvcAuthenticationStateProvider>()
            .AddScoped(sp => (SvcAuthenticationStateProvider)sp.GetRequiredService<BlazorAuthenticationStateProvider>());
}
