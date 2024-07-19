using ActualLab.Fusion.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

namespace SpotifyVoiceCommander.Maui.Shared.UI;

public abstract class SvcComponentFluxorBase : SvcComponentBase, ISvcFluxorComponentSubscriber
{
    #region Injects

    // Fluxor
    [Inject] private SvcFluxorActionResolver _svcFluxorActionResolver { get; init; } = null!;

    // State's
    [Inject] protected HostInfo _hostInfo { get; init; } = null!;

    // Blazor 
    [Inject] protected BackendUrlMapper _backendUrlMapper { get; init; } = null!;
    [Inject] protected IJSRuntime _jSRuntime { get; init; } = null!;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        _svcFluxorActionResolver.SetSubscriber(this);   
    }

    #endregion

    #region Fluxor

    public SvcFluxorSubscription<TAction> SubscribeTo<TAction>() where TAction : ISvcAction =>
        _svcFluxorActionResolver.SubscribeTo<TAction>();

    public ErrorOr<Success> Dispatch<TAction>(TAction action, CancellationToken ct = default) where TAction : ISvcAction =>
        action.ToErrorOr()
            .ThenDo(action => _svcFluxorActionResolver.Dispatch(action, ct))
            .Then(_ => Result.Success);

    public BlazorDispatcher GetBlazorDispatcher() => this.GetDispatcher();
    public void CallStateHasChanged() => this.NotifyStateHasChanged();

    #endregion
}
