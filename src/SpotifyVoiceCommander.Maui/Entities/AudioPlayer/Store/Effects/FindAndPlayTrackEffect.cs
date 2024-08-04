using SpotifyAPI.Web;
using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

namespace SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Effects;

internal class FindAndPlayTrackEffect(
    IServiceProvider _services,
    SpotifyClientWrapper _spotifyClientWrapper) 
    : BaseEffect<FindAndPlayTrackAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<FindAndPlayTrackAction>> actionWrapper) => actionWrapper
        .Then(aw => _spotifyClientWrapper.SpotifyClient
            .Then(sc => (SpotifyClient: sc, ActionWrapper: aw)))
        .ThenAsync(async s => (
            SearchResult: await s.SpotifyClient.Search.Item(
                new SearchRequest(
                    SearchRequest.Types.Track,
                    s.ActionWrapper.Action.TrackFullName),
                CancellationToken),
            s.SpotifyClient))
        .Then(s => (BestMatch: s.SearchResult.Tracks.Items?.FirstOrDefault(), s.SpotifyClient))
        .FailIf(
            s => s.BestMatch == null,
            _ => Error.NotFound())
        .ThenDoAsync(s => s.SpotifyClient.Player
            .SafeAddToQueue(new PlayerAddToQueueRequest($"spotify:track:{s.BestMatch!.Id}"), CancellationToken)
            .ContinueWith(_ => s.SpotifyClient.Player.SafeSkipNext(CancellationToken)))
        .ThenDo(s => Dispatch(new FindAndPlayTrackSuccessAction
        {
            Track = s.BestMatch!,
        }))
        .Else(errors => HandleErrorState(
            errors,
            new FindAndPlayTrackFailureAction { },
            Error.NotFound()));
}
