using SpotifyVoiceCommander.Maui.Entities.Viewer.Store;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Queues;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Middlewares;

internal sealed class AuthenticateStateCheckMiddleware(
    IServiceProvider _services,
    IState<ViewerState> _viewerState) : Middleware
{
    private AfterAuthenticationActionQueueDispatcher _afterAuthenticationActionQueueDispatcher = null!;

    public override void AfterInitializeAllMiddlewares() =>
        _afterAuthenticationActionQueueDispatcher = _services.GetRequiredService<AfterAuthenticationActionQueueDispatcher>();

    public override bool MayDispatchAction(object action)
    {
        bool isEtaiActionRequiredUser = action is ISvcAction && action is not IAllowAnonymousAction;
        if (isEtaiActionRequiredUser && !_viewerState.Value.HasAuthenticatedViewer)
        {
            _afterAuthenticationActionQueueDispatcher.Add((ISvcAction)action);
            return false;
        }

        return base.MayDispatchAction(action);
    }
}
