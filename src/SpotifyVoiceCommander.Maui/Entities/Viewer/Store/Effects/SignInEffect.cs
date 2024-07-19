using SpotifyVoiceCommander.Maui.Entities.Viewer.Models;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Effects;

internal class SignInEffect(
    IServiceProvider _services,
    SpotifyClientWrapper _spotifyClientWrapper) 
    : BaseEffect<SignInAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<SignInAction>> actionWrapper) => actionWrapper
        .ThenAsync(_ => _spotifyClientWrapper.SignInAsync())
        .Switch(
            _ => Dispatch(new SignInSuccessAction { }),
            errors => ShowErrorMessage(errors)
                .ThenDo(_ => Dispatch(new SignInFailureAction { }))); 
}
