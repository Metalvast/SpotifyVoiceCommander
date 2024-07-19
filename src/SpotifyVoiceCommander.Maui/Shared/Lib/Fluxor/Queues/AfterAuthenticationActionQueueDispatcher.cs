using SpotifyVoiceCommander.Maui.Entities.Viewer.Store;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Queues;

internal class AfterAuthenticationActionQueueDispatcher : ISvcFluxorSubscriber, IDisposable
{
    #region Injects

    private readonly SvcFluxorActionResolver _actionResolver;
    private readonly IState<ViewerState> _viewerState;

    #endregion

    #region Ctors

    public AfterAuthenticationActionQueueDispatcher(
        SvcFluxorActionResolver actionResolver,
        IState<ViewerState> viewerState)
    {
        _actionResolver = actionResolver;
        _viewerState = viewerState;
        TagId = new(nameof(AfterAuthenticationActionQueueDispatcher));

        _actionResolver.SetSubscriber(this);
        _actionResolver.SubscribeTo<InitializeViewerFailureAction>()
            .WithHandler(_ => _actionQueue.Clear());
        _actionResolver.SubscribeTo<InitializeViewerSuccessAction>()
            .WithHandler(OnInitializeViewerSuccessAction);
    }

    #endregion

    #region Fields

    private readonly Queue<ISvcAction> _actionQueue = [];

    public TagId TagId { get; }

    #endregion

    #region External events

    private void OnInitializeViewerSuccessAction(FluxorActionWrapper<InitializeViewerSuccessAction> action)
    {
        if (!_viewerState.Value.HasAuthenticatedViewer)
        {
            _actionQueue.Clear();
            return;
        }

        while (_actionQueue.Count != 0)
            _actionResolver.Dispatch(_actionQueue.Dequeue());
    }

    #endregion

    #region Public

    public void Add(ISvcAction action) =>
        _actionQueue.Enqueue(action);

    public void Dispose() =>
        _actionQueue.Clear();

    #endregion
}
