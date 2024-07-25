using SpotifyAPI.Web;

namespace SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store;

[FeatureState]
public record AudioPlayerState
{
    public ErrorOr<FullTrack> FullTrack { get; init; }
}
