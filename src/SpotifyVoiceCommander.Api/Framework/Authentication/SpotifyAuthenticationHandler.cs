using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SpotifyVoiceCommander.Api.Framework.Authentication;

public class SpotifyAuthenticationHandler : AuthenticationHandler<SpotifyAuthenticationOptions>
{
    #region Injects

    private readonly ILogger<SpotifyAuthenticationHandler> _logger;
    private readonly RestClient _restClient;

    #endregion

    #region Ctors

    public SpotifyAuthenticationHandler(
        IOptionsMonitor<SpotifyAuthenticationOptions> _options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        RestClient restClient)
        : base(_options, loggerFactory, encoder)
    {
        _logger = loggerFactory.CreateLogger<SpotifyAuthenticationHandler>();
        _restClient = restClient;
    }

    #endregion

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderStringValue))
                return AuthenticateResult.Fail("Access token not provided");

            var restRequest = new RestRequest("api/v1/me")
                .AddHeader("Authorization", authorizationHeaderStringValue.ToString());
            var restResponse = await _restClient.ExecuteAsync(restRequest);
            if (!restResponse.IsSuccessStatusCode)
                return AuthenticateResult.Fail("Token validation failed");

            var jsonDocument = JsonDocument.Parse(restResponse.Content!);
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, jsonDocument.RootElement.GetProperty("id").GetString()!),
                new(ClaimTypes.Name, jsonDocument.RootElement.GetProperty("display_name").GetString()!),
            };
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed with unhandle error");
            return AuthenticateResult.Fail("Token validation failed with unhandle error");
        }
    }
}