using Auth0.OidcClient;
using IdentityModel.OidcClient.Browser;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using SpotifyVoiceCommander.Maui.Shared.Api.Lib;
using SpotifyVoiceCommander.Maui.Shared.Lib.AuthenticationStateProvider;
using System.Security.Claims;
using System.Text.Json;
using System.Web;
using LoginRequest = SpotifyAPI.Web.LoginRequest;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

public sealed class SpotifyClientWrapper(
    InternalApiAuthenticator _internalApiAuthenticator,
    SpotifyClientHttpMessageHandler _spotifyClientHttpMessageHandler)
    : INeedInitialization,
    IDisposable
{
    #region Const

    public static readonly ClaimsPrincipal AnonymousUser = new(new ClaimsIdentity());

    #endregion

    #region Fields

    private SpotifyClient? _spotifyClient;
    private WebAuthenticatorBrowser _authenticationBrowser = new();
    private OAuthClient _oAuthClient = new();
    private PKCEAuthenticator? _pkceAuthenticator;

    #endregion

    public bool MustAwait => false;

    public ErrorOr<SpotifyClient> SpotifyClient =>
        _spotifyClient != null
            ? _spotifyClient.ToErrorOr()
            : SvcErrors.NotInitialized;

    public event Action<ClaimsPrincipal>? OnAuthenticationStateChanged;

    public async ValueTask InitializeAsync()
    {
        await Task.Yield();

        var authInfoJson = await SecureStorage.GetAsync(SecureStorageKeys.AuthInfo);
        if (authInfoJson == null ||
            JsonSerializer.Deserialize<PKCETokenResponse>(authInfoJson) is not { } authInfo)
        {
            OnAuthenticationStateChanged?.Invoke(AnonymousUser);
            return;
        }

        await InitializerSpotifyClientAsync(authInfo);
    }

    public async Task<ErrorOr<Success>> SignInAsync(CancellationToken ct = default)
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes(120);
        var loginUrl = CreateLoginUri(challenge);
        var callbackUrl = CreateCallbackUri();

        var browserAuthenticationResult = await _authenticationBrowser.InvokeAsync(new BrowserOptions(loginUrl, callbackUrl), ct);
        if (browserAuthenticationResult.IsError)
            return SvcErrors.InitializationFailed;

        var code = HttpUtility.ParseQueryString(new Uri(browserAuthenticationResult.Response).Query).Get("code");
        if (code == null)
            return SvcErrors.InitializationFailed;

        var authInfo = await _oAuthClient.RequestToken(
            new PKCETokenRequest(
                Constants.SpotifyClientId,
                code,
                new Uri(callbackUrl),
                verifier),
            ct);
        await InitializerSpotifyClientAsync(authInfo);
        await SetAuthInfo(authInfo);

        return Result.Success;
    }

    public void SignOut()
    {
        _internalApiAuthenticator.Clear();
        _spotifyClient = null;
        if (_pkceAuthenticator != null)
        {
            _pkceAuthenticator.TokenRefreshed -= OnTokenRefreshed;
            _pkceAuthenticator = null;
        }

        SecureStorage.Remove(SecureStorageKeys.AuthInfo);
        OnAuthenticationStateChanged?.Invoke(AnonymousUser);
    }

    public void Dispose()
    {
        _internalApiAuthenticator.Clear();
        
        if (_pkceAuthenticator != null)
        {
            _pkceAuthenticator.TokenRefreshed -= OnTokenRefreshed;
            _pkceAuthenticator = null;
        }

        _spotifyClient = null;
        _authenticationBrowser = null!;
        _oAuthClient = null!;   
    }

    #region External events

    private void OnTokenRefreshed(object? _, PKCETokenResponse authInfo) =>
        SetAuthInfo(authInfo);

    #endregion

    #region Private methods

    private static string CreateLoginUri(string challenge) =>
        new LoginRequest(
            new Uri(CreateCallbackUri()),
            Constants.SpotifyClientId,
            LoginRequest.ResponseType.Code)
        {
            CodeChallengeMethod = "S256",
            CodeChallenge = challenge,
            Scope =
            [
                Scopes.AppRemoteControl,
                Scopes.PlaylistModifyPrivate,
                Scopes.PlaylistModifyPublic,
                Scopes.PlaylistReadCollaborative,
                Scopes.PlaylistReadPrivate,
                Scopes.Streaming,
                Scopes.UgcImageUpload,
                Scopes.UserFollowModify,
                Scopes.UserFollowRead,
                Scopes.UserLibraryModify,
                Scopes.UserLibraryRead,
                Scopes.UserModifyPlaybackState,
                Scopes.UserReadCurrentlyPlaying,
                Scopes.UserReadEmail,
                Scopes.UserReadPlaybackPosition,
                Scopes.UserReadPlaybackState,
                Scopes.UserReadPrivate,
                Scopes.UserReadRecentlyPlayed,
                Scopes.UserTopRead,
            ],
        }.ToUri().ToString();

    private static string CreateCallbackUri() =>
        "spotifyvoicecommanderapp://callback/";

    private static Task SetAuthInfo(PKCETokenResponse authInfo) =>
        SecureStorage.SetAsync(SecureStorageKeys.AuthInfo, JsonSerializer.Serialize(authInfo));

    private async Task InitializerSpotifyClientAsync(PKCETokenResponse authInfo, CancellationToken ct = default)
    {
        _pkceAuthenticator = new PKCEAuthenticator(Constants.SpotifyClientId, authInfo);
        _pkceAuthenticator.TokenRefreshed += OnTokenRefreshed;
        var config = SpotifyClientConfig.CreateDefault()
           .WithAuthenticator(_pkceAuthenticator)
           .WithHTTPClient(new NetHttpClient(new HttpClient(_spotifyClientHttpMessageHandler)));

        _spotifyClient = new SpotifyClient(config);
        _internalApiAuthenticator.SetSpotifyAuthenticationProxies(_pkceAuthenticator, _spotifyClient.GetApiConnector());
        var user = await _spotifyClient.UserProfile.Current(ct);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.DisplayName),
        };
        var claimsIdentity = new ClaimsIdentity(claims, "oauth2");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        OnAuthenticationStateChanged?.Invoke(claimsPrincipal);
    }

    #endregion
}
