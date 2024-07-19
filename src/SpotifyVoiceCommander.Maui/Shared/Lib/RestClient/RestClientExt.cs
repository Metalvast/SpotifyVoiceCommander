using RestSharp;
using RestSharp.Authenticators;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.RestClient;

internal static class RestClientExt
{
    public static RestRequest WithAuthenticator(this RestRequest request, IAuthenticator authenticator)
    {
        request.Authenticator = authenticator;
        return request;
    }

    public static RestRequest TryWithAuthenticator(this RestRequest request, IAuthenticator authenticator)
    {
        request.Authenticator ??= authenticator;
        return request;
    }
}
