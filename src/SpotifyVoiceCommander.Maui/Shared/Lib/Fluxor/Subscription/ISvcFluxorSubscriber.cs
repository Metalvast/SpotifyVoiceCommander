namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

public interface ISvcFluxorSubscriber
{
    public TagId TagId { get; }
}

public interface ISvcFluxorComponentSubscriber : ISvcFluxorSubscriber
{
    public BlazorDispatcher GetBlazorDispatcher();
    public void CallStateHasChanged();
}