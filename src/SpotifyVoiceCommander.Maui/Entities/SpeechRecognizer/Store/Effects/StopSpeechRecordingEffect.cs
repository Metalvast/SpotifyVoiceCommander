using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class StopSpeechRecordingEffect(
    IServiceProvider _services,
    IState<SpeechRecognizerState> _speechRecognizerState)
    : BaseEffect<StopSpeechRecordingAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<StopSpeechRecordingAction>> actionWrapper) => actionWrapper
        .FailIf(
            _ => !_speechRecognizerState.Value.IsRecording,
            _ => Error.Conflict())
        .Then(_ => _speechRecognizerState.Value.AudioRecorder)
        .ThenAsync(audioRecorder => audioRecorder.StopAsync())
        .ThenDo(_ => Dispatch(new StopSpeechRecordingSuccessAction { }))
        .Else(errors => HandleErrorState(
            errors,
            new StopSpeechRecordingFailureAction { },
            Error.Unexpected()));
}
