using Microsoft.AspNetCore.Authentication;

namespace SpotifyVoiceCommander.Api.Framework.Authentication;

public class SpotifyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "SpotifyCustomAuthentication";
    public const string AuthorizationHeaderName = "Authorization";
}
