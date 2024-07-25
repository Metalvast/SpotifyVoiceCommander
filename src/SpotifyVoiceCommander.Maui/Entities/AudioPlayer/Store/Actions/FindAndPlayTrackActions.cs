using SpotifyAPI.Web;

namespace SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;

public record FindAndPlayTrackAction : ISvcAction
{
    public required string TrackFullName { get; init; }
}
public record FindAndPlayTrackFailureAction : ISvcAction;
public record FindAndPlayTrackSuccessAction : ISvcAction
{
    public required FullTrack Track { get; init; }
}
