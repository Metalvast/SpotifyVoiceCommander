using Microsoft.AspNetCore.Authentication;
using RestSharp;

namespace SpotifyVoiceCommander.Api.Framework.Authentication;

internal static class SpotifyAuthenticationConfiguration
{
    public static AuthenticationBuilder AddCustomSpotifyScheme(this AuthenticationBuilder builder)
    {
        builder.Services.AddSingleton(new RestClient(new HttpClient
        {
            BaseAddress = new Uri("https://api.spotify.com/")
        }));

        builder.AddScheme<SpotifyAuthenticationOptions, SpotifyAuthenticationHandler>(
            SpotifyAuthenticationOptions.DefaultScheme,
            options => { });

        return builder;
    }
}
