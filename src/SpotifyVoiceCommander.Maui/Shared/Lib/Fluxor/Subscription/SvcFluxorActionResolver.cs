namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

internal class SvcFluxorActionResolver(
    IActionSubscriber _actionSubscriber,
    IDispatcher _dispatcher)
    : IDisposable
{
    #region Fields

    private readonly List<ISvcFluxorSubscription> _subscriptions = [];
    private ISvcFluxorSubscriber? _subscriber;

    #endregion

    #region Public

    public void SetSubscriber(ISvcFluxorSubscriber subscriber) =>
        _subscriber = subscriber;

    public SvcFluxorSubscription<TAction> SubscribeTo<TAction>() where TAction : ISvcAction
    {
        var subscription = new SvcFluxorSubscription<TAction>(
            EnsureSubscriberExist(),
            _actionSubscriber);

        _subscriptions.Add(subscription);
        return subscription;
    }

    public void Dispatch<TAction>(TAction action, CancellationToken ct = default) where TAction : ISvcAction =>
        _dispatcher.Dispatch(new FluxorActionWrapper<TAction>
        {
            Action = action,
            TagId = EnsureSubscriberExist().TagId,
            CancellationToken = ct,
        });

    public void Dispose()
    {
        _subscriptions.Clear();
        if (_subscriber != null)
        {
            _actionSubscriber.UnsubscribeFromAllActions(_subscriber);
            _subscriber = null;
        }
    }

    #endregion

    #region Private methods

    private ISvcFluxorSubscriber EnsureSubscriberExist() =>
        _subscriber ?? throw new InvalidOperationException("Subscriber not exist");

    #endregion
}
