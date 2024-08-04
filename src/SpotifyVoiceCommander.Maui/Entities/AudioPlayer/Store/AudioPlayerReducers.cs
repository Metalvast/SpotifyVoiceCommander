using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store;

internal static class AudioPlayerReducers
{
    [ReducerMethod]
    public static AudioPlayerState ReduceFindAndPlayTrackAction(
        AudioPlayerState state,
        FluxorActionWrapper<FindAndPlayTrackAction> _) =>
        state with
        {
            FullTrack = SvcErrors.NotInitialized,
            SearchState = LoaderState.Loading,
        };

    [ReducerMethod]
    public static AudioPlayerState ReduceFindAndPlayTrackFailureAction(
        AudioPlayerState state,
        FluxorActionWrapper<FindAndPlayTrackFailureAction> _) =>
        state with
        {
            FullTrack = Error.Unexpected(),
            SearchState = LoaderState.Error,
        };

    [ReducerMethod]
    public static AudioPlayerState ReduceFindAndPlayTrackSuccessAction(
        AudioPlayerState state,
        FluxorActionWrapper<FindAndPlayTrackSuccessAction> actionWrapper) =>
        state with
        {
            FullTrack = actionWrapper.Action.Track,
            SearchState = LoaderState.Content,
        };
}
