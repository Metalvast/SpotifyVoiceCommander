using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Models;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;
using SpotifyVoiceCommander.Shared.Models.RecognizeSpeech;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class RecognizeSpeechEffect(IServiceProvider _services) : BaseEffect<RecognizeSpeechAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<RecognizeSpeechAction>> actionWrapper) => actionWrapper
        .ThenAsync(aw => _httpEndpointsClient.YandexEndpoints
            .RecognizeSpeechAsync(
                new RecognizeSpeechRequest
                {
                    AudioData = aw.Action.AudioData
                },
                aw.CancellationToken)
            .Then(recognizeResponse => (RecognizeResult: recognizeResponse.Result, aw.CancellationToken)))
        .FailIf(
            recognizeResponse => recognizeResponse.RecognizeResult.IsNullOrEmpty(),
            _ => SpeechRecognizerErrors.RecognizeFailed)
        .ThenAsync(recognizeResponse => _httpEndpointsClient.YandexEndpoints.AnalyzeSpeechAsync(
            new AnalyzeSpeechRequest
            {
                RecognizedSpeech = recognizeResponse.RecognizeResult!,
            },
            recognizeResponse.CancellationToken))
        .ThenDo(analyzeResponse => Dispatch(new RecognizeSpeechSuccessAction
        {
            Result = analyzeResponse.Result,
        }))
        .ThenDo(analyzeResponse => Dispatch(new FindAndPlayTrackAction
        {
            TrackFullName = analyzeResponse.Result.Alternatives[0].Message.Text,
        }))
        .Else(errors => HandleErrorState(
            errors,
            new RecognizeSpeechFailureAction { },
            SpeechRecognizerErrors.AnalyzeFailed));
}
