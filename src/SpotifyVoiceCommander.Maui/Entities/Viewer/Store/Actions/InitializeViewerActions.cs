using Microsoft.AspNetCore.Components.Authorization;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Models;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

public record InitializeViewerAction : ISvcAction, IAllowAnonymousAction
{
    public required Task<AuthenticationState> AuthenticationStateTask { get; init; }
}
public record InitializeViewerFailureAction : ISvcAction, IAllowAnonymousAction;
public record InitializeViewerSuccessAction : ISvcAction, IAllowAnonymousAction
{
    public ViewerModel? Viewer { get; init; }
}