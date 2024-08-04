using ActualLab.Async;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;
using System.Security.Claims;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AuthenticationStateProvider;

public class SvcAuthenticationStateProvider : BlazorAuthenticationStateProvider, IDisposable
{
    #region Injects

    private readonly ILogger<SvcAuthenticationStateProvider> _logger;
    private readonly SpotifyClientWrapper _spotifyClientWrapper;
    private readonly IDispatcher _dispatcher;

    #endregion

    #region Ctors

    public SvcAuthenticationStateProvider(
        ILogger<SvcAuthenticationStateProvider> logger,
        SpotifyClientWrapper spotifyClientWrapper,
        IDispatcher dispatcher)
    {
        _logger = logger;
        _spotifyClientWrapper = spotifyClientWrapper;
        _dispatcher = dispatcher;

        _spotifyClientWrapper.OnAuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    #endregion

    #region Fields

    private AuthenticationState _currentUser = new(SpotifyClientWrapper.AnonymousUser);
    private readonly TaskCompletionSource<AuthenticationState> _initialStateTaskSource = TaskCompletionSourceExt.New<AuthenticationState>();
    public TaskCompletionSource<AuthenticationState> WhenFirstTimeAuthenticated = TaskCompletionSourceExt.New<AuthenticationState>();

    #endregion

    #region External events

    private void OnAuthenticationStateChanged(ClaimsPrincipal user)
    {
        _logger.LogDebug($"Start");

        var newUserId = user.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
        _logger.LogDebug("New UserId: {newUserId}", newUserId);

        var currentUserId = _currentUser.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
        _logger.LogDebug("Current UserId: {currentUserId}", currentUserId);

        bool isSameUser = newUserId?.Value?.Equals(currentUserId?.Value) ?? false;
        _logger.LogDebug("Is same user: {isSameUser}", isSameUser);

        bool notAnonymous = user.Identity?.IsAuthenticated ?? false;
        _logger.LogDebug("Not anonymous: {notAnonymous}", notAnonymous);

        _currentUser = new AuthenticationState(user);
        var authenticationStateTask = Task.FromResult(_currentUser);

        if (_initialStateTaskSource.Task.Status is TaskStatus.WaitingForActivation)
            _initialStateTaskSource.TrySetResult(_currentUser);

        if (notAnonymous)
        {
            if (!WhenFirstTimeAuthenticated.Task.IsCompleted)
                WhenFirstTimeAuthenticated.SetResult(_currentUser);

            if (!isSameUser)
                Notify();
        }
        else
            Notify();

        _logger.LogDebug($"End");

        void Notify()
        {
            NotifyAuthenticationStateChanged(authenticationStateTask);
            _dispatcher.Dispatch(new FluxorActionWrapper<InitializeViewerAction>
            {
                Action = new InitializeViewerAction { AuthenticationStateTask = authenticationStateTask },
            });
        }
    }

    #endregion

    #region Public

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        _initialStateTaskSource.Task.Status is TaskStatus.WaitingForActivation
            ? _initialStateTaskSource.Task
            : Task.FromResult(_currentUser);

    public void Dispose() =>
        _spotifyClientWrapper.OnAuthenticationStateChanged -= OnAuthenticationStateChanged;

    #endregion
}
