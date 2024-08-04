using SpotifyVoiceCommander.Maui.Shared.Api.Lib;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

public sealed class SpotifyClientHttpMessageHandler(InternalApiUrlMapper _backendUrlMapper) : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        UriBuilder ub = new(request.RequestUri!);
        ub.Path = $"/spotify/{GetSpotifyServiceUriPart(ub.Host)}{ub.Path}";
        ub.Host = _backendUrlMapper.ApiBaseUri.Host;
        ub.Port = _backendUrlMapper.ApiBaseUri.Port;
        request.RequestUri = ub.Uri;

        return base.SendAsync(request, cancellationToken);
    }

    private static string GetSpotifyServiceUriPart(string host) =>
        true switch
        {
            _ when host.StartsWith("accounts.spotify.com") => "accounts",
            _ => "api",
        };
}
