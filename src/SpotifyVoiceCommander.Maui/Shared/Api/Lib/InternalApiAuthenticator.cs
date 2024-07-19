using RestSharp;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

namespace SpotifyVoiceCommander.Maui.Shared.Api.Lib;

public sealed class InternalApiAuthenticator : RestSharp.Authenticators.IAuthenticator
{
    #region Injects

    private PKCEAuthenticator? _pkceAuthenticator;
    private IAPIConnector? _apiConnector;

    #endregion

    #region Public

    public void SetSpotifyAuthenticationProxies(
        PKCEAuthenticator pkceAuthenticator,
        IAPIConnector apiConnector)
    {
        _pkceAuthenticator = pkceAuthenticator;
        _apiConnector = apiConnector;
    }

    public void Clear()
    {
        _pkceAuthenticator = null;
        _apiConnector = null;
    }

    public ValueTask Authenticate(IRestClient client, RestRequest request)
    {
        if (_pkceAuthenticator == null ||
            _apiConnector == null)
            return ValueTask.CompletedTask;

        _pkceAuthenticator.Apply(SpotifyEmptyRequest.Default, _apiConnector);
        request.AddHeader("Authorization", SpotifyEmptyRequest.Default.Headers.Values.First());
        return ValueTask.CompletedTask;
    }

    #endregion
}