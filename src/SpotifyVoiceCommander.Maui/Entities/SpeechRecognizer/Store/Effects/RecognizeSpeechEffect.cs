using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Models;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;
using SpotifyVoiceCommander.Shared.Models.RecognizeSpeech;
using System.Text.RegularExpressions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class RecognizeSpeechEffect(IServiceProvider _services) : BaseEffect<RecognizeSpeechAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<RecognizeSpeechAction>> actionWrapper) => actionWrapper
        .ThenAsync(aw => _httpEndpointsClient.YandexEndpoints.RecognizeSpeechAsync(
            new RecognizeSpeechRequest
            {
                AudioData = aw.Action.AudioData
            },
            CancellationToken))
        .FailIf(
            recognizeResponse => recognizeResponse.Result.IsNullOrEmpty(),
            _ => SpeechRecognizerErrors.RecognizeFailed)
        .ThenAsync(recognizeResponse => _httpEndpointsClient.YandexEndpoints.AnalyzeSpeechAsync(
            new AnalyzeSpeechRequest
            {
                RecognizedSpeech = recognizeResponse.Result!,
            },
            CancellationToken))
        .ThenDo(analyzeResponse => Dispatch(new RecognizeSpeechSuccessAction
        {
            Result = analyzeResponse.Result,
        }))
        .Then(analyzeResponse => analyzeResponse.Result.Alternatives[0].Message.Text)
        .Then(trackName => Regex.Replace(trackName, @"[\«\»]+", ""))
        .ThenDo(trackName => _logger.LogDebug("Analyze result: {Result}", trackName))
        .ThenDo(trackName => Dispatch(new FindAndPlayTrackAction
        {
            TrackFullName = trackName,
        }))
        .Else(errors => HandleErrorState(
            errors,
            new RecognizeSpeechFailureAction { },
            SpeechRecognizerErrors.AnalyzeFailed));
}
