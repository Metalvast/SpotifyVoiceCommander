using SpotifyVoiceCommander.Maui.Entities.Viewer.Models;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using System.Security.Claims;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Effects;

internal class InitializeViewerEffect(IServiceProvider services) : BaseEffect<InitializeViewerAction>(services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<InitializeViewerAction>> actionWrapper) => actionWrapper
        .ThenAsync(aw => aw.Action.AuthenticationStateTask)
        .FailIf(
            authenticationState => !authenticationState.User.Identity!.IsAuthenticated,
            authenticationState => Dispatch(new InitializeViewerSuccessAction { })
                .Then(_ => Error.Unauthorized())
                .Value)
        .Then(authenticationState => new ViewerModel
        {
            Id = authenticationState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
            Name = authenticationState.User.Identity!.Name ?? "User",
        })
        .ThenDo(viewer => Dispatch(new InitializeViewerSuccessAction
        {
            Viewer = viewer,
        }))
        .Else(errors => ShowErrorMessage(errors)
            .ThenDo(_ => Dispatch(new InitializeViewerFailureAction { }))
            .Then(_ => Error.Unauthorized())
            .Value);
}
