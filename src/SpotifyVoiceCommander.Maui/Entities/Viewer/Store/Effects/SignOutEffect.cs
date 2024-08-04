using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Effects;

internal class SignOutEffect(
    IServiceProvider _services,
    SpotifyClientWrapper _spotifyClientWrapper)
    : BaseEffect<SignOutAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<SignOutAction>> actionWrapper) => actionWrapper
        .ThenDo(_ => _spotifyClientWrapper.SignOut())
        .ThenDo(_ => _navigationManager.NavigateToSignIn(forceReload: true))
        .ThenDoAsync(_ => Task.CompletedTask);
}
