using Microsoft.Extensions.Logging;
using RestSharp;
using SpotifyVoiceCommander.Maui.Shared.Api.Lib;
using SpotifyVoiceCommander.Maui.Shared.Api.Shell.Areas;
using SpotifyVoiceCommander.Maui.Shared.Lib.RestClient;
using System.Net;
using System.Text.Json;

namespace SpotifyVoiceCommander.Maui.Shared.Api.Shell;

public sealed class HttpEndpointsClient
{
    #region Injects

    private readonly InternalApiAuthenticator _internalApiAuthenticator;
    private readonly ILogger<HttpEndpointsClient> _logger;

    #endregion

    #region Ctors

    public HttpEndpointsClient(
        RestClient restClient,
        InternalApiAuthenticator internalApiAuthenticator,
        ILogger<HttpEndpointsClient> logger)
    {
        RestClient = restClient;
        _internalApiAuthenticator = internalApiAuthenticator;
        _logger = logger;
    }

    #endregion

    #region Areas

    private YandexEndpoints? _yandexEndpoints;
    public YandexEndpoints YandexEndpoints =>
        _yandexEndpoints ??= new YandexEndpoints(this);

    #endregion

    #region Internal 

    internal RestClient RestClient { get; }

    internal Task<ErrorOr<T>> SafeExecuteAsync<T>(RestRequest restRequest, CancellationToken ctn = default) where T : class =>
        restRequest
            .ToErrorOr()
            .Then(request => request.TryWithAuthenticator(_internalApiAuthenticator))
            .ThenAsync(request => RestClient.ExecuteAsync(request, ctn))
            .FailIf(response => !response.IsSuccessful, GetError)
            .Then(response => JsonSerializer.Deserialize<T>(response.Content!, Constants.CamelCaseJsonSerializerOptions)!);

    #endregion

    #region Private methods

    private Error GetError(RestResponse response) =>
        true switch
        {
            _ when response.StatusCode is HttpStatusCode.BadRequest => Error.Validation(),
            _ when response.StatusCode is HttpStatusCode.Unauthorized => Error.Unauthorized(),
            _ when response.StatusCode is HttpStatusCode.Forbidden => Error.Forbidden(),
            _ when response.StatusCode is HttpStatusCode.NotFound => Error.NotFound(),
            _ when response.StatusCode is HttpStatusCode.InternalServerError =>
                Error.Custom((int)response.StatusCode, "General.InternalServerError", "Something happend on server"),
            _ when response.StatusCode is HttpStatusCode.BadGateway =>
                Error.Custom((int)response.StatusCode, "General.BadGateway", "Bad gateway"),
            _ when response.StatusCode is HttpStatusCode.ServiceUnavailable =>
                Error.Custom((int)response.StatusCode, "General.ServiceUnavailable", "Service unavailable"),
            _ when (int)response.StatusCode == 501 || (int)response.StatusCode > 503 =>
                Error.Custom((int)response.StatusCode, "General.ServerError", "Server has some problems"),
            _ => Error.Unexpected(),
        };

    #endregion
}
