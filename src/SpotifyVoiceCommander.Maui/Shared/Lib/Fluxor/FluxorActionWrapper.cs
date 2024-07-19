namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor;

public record FluxorActionWrapper<T> where T : ISvcAction
{
    public required T Action { get; init; }

    public TagId? TagId { get; init; }
    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;
}
