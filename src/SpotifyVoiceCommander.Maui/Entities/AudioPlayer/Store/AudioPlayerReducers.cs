using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store;

internal static class AudioPlayerReducers
{
    [ReducerMethod]
    public static AudioPlayerState Reduce(
        AudioPlayerState state,
        FluxorActionWrapper<FindAndPlayTrackSuccessAction> actionWrapper) =>
        state with
        {
            FullTrack = actionWrapper.Action.Track,
        };
}
