namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

public class SvcFluxorSubscription<TAction> : ISvcFluxorSubscription
    where TAction : ISvcAction
{
    #region Ctors

    public SvcFluxorSubscription(
        ISvcFluxorSubscriber subcriber,
        IActionSubscriber actionSubscriber)
    {
        _tagId = subcriber.TagId;
        actionSubscriber.SubscribeToAction(subcriber, delegate (FluxorActionWrapper<TAction> actionWrapper) {

            if (!CheckAllConditions(actionWrapper))
                return;

            if (subcriber is not ISvcFluxorComponentSubscriber componentSubscriber || !_mustRender)
            {
                ExecuteAllSyncHandlers(actionWrapper);
                _ = ExecuteAllAsyncHandlers(actionWrapper);
                return;
            }

            componentSubscriber
                .GetBlazorDispatcher()
                .InvokeAsync(async delegate {
                    ExecuteAllSyncHandlers(actionWrapper);
                    await ExecuteAllAsyncHandlers(actionWrapper);
                    componentSubscriber.CallStateHasChanged();
                });
        });
    }

    #endregion

    #region Fields

    private readonly TagId _tagId;
    private readonly List<Action<FluxorActionWrapper<TAction>>> _syncHandlers = [];
    private readonly List<Func<FluxorActionWrapper<TAction>, Task>> _asyncHandlers = [];
    private readonly List<Func<FluxorActionWrapper<TAction>, bool>> _conditions = [];
    private bool _mustRender = true;

    #endregion

    #region Pipe methods

    public SvcFluxorSubscription<TAction> WithHandler(Action<FluxorActionWrapper<TAction>> syncHandler)
    {
        _syncHandlers.Add(syncHandler);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithHandler(Func<FluxorActionWrapper<TAction>, Task> asyncHanlder)
    {
        _asyncHandlers.Add(asyncHanlder);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithCondition(Func<FluxorActionWrapper<TAction>, bool> condition)
    {
        _conditions.Add(condition);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithTagReceiverCondition()
    {
        _conditions.Add(action => action.TagId == _tagId);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithoutRender()
    {
        _mustRender = false;
        return this;
    }

    #endregion

    #region Private methods

    private bool CheckAllConditions(FluxorActionWrapper<TAction> actionWrapper) =>
        _conditions.All(condition => condition.Invoke(actionWrapper));

    private void ExecuteAllSyncHandlers(FluxorActionWrapper<TAction> actionWrapper) =>
        _syncHandlers.ForEach(syncHandler => syncHandler.Invoke(actionWrapper));

    private Task ExecuteAllAsyncHandlers(FluxorActionWrapper<TAction> actionWrapper) =>
        Task.WhenAll(_asyncHandlers.Select(asyncHandler => asyncHandler.Invoke(actionWrapper)));

    #endregion
}
