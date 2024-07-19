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
                ExecuteAllHandlers(actionWrapper);
                return;
            }

            componentSubscriber
                .GetBlazorDispatcher()
                .InvokeAsync(delegate {
                    ExecuteAllHandlers(actionWrapper);
                    componentSubscriber.CallStateHasChanged();
                });
        });
    }

    #endregion

    #region Fields

    private readonly TagId _tagId;
    private readonly List<Action<FluxorActionWrapper<TAction>>> _handlers = [];
    private readonly List<Func<FluxorActionWrapper<TAction>, bool>> _conditions = [];
    private bool _mustRender = true;

    #endregion

    #region Pipe methods

    public SvcFluxorSubscription<TAction> WithHandler(Action<FluxorActionWrapper<TAction>> hanlder)
    {
        _handlers.Add(hanlder);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithCondition(Func<FluxorActionWrapper<TAction>, bool> condition)
    {
        _conditions.Add(condition);
        return this;
    }

    public SvcFluxorSubscription<TAction> WithComponentReceiverCondition()
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

    private void ExecuteAllHandlers(FluxorActionWrapper<TAction> actionWrapper)
    {
        foreach (var handler in _handlers)
            handler.Invoke(actionWrapper);
    }

    #endregion
}
