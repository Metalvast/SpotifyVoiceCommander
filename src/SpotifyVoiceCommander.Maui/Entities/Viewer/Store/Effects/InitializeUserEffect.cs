﻿using SpotifyVoiceCommander.Maui.Entities.Viewer.Models;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;
using System.Security.Claims;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Effects;

internal class InitializeViewerEffect(IServiceProvider services) : BaseEffect<InitializeViewerAction>(services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<InitializeViewerAction>> actionWrapper) => actionWrapper
        .ThenAsync(aw => aw.Action.AuthenticationStateTask)
        .FailIf(
            authenticationState => !authenticationState.User.Identity!.IsAuthenticated,
            _ => Error.Unauthorized())
        .Then(authenticationState => new ViewerModel
        {
            Id = authenticationState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
            Name = authenticationState.User.Identity!.Name ?? "User",
        })
        .ThenDo(viewer => Dispatch(new InitializeViewerSuccessAction
        {
            Viewer = viewer,
        }))
        .Switch(_ => _navigationManager.NavigateToPlayer(), HandleErrors);

    private void HandleErrors(IEnumerable<Error> errors)
    {
        if (errors.Contains(Error.Unauthorized()))
        {
            Dispatch(new InitializeViewerSuccessAction
            {
                Viewer = null,
            });
            return;
        }

        HandleErrorState(
            errors,
            new InitializeViewerFailureAction { },
            Error.Unexpected());
    }
}