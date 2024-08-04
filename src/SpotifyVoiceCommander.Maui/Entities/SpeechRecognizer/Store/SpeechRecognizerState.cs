using Plugin.Maui.Audio;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

[FeatureState]
public record SpeechRecognizerState
{
    public LoaderState InitializingState { get; init; }
    public ErrorOr<IAudioRecorder> AudioRecorder { get; init; }
    public Error LastError { get; init; }

    public bool IsTryingRecognize { get; init; }

    public bool IsBusy =>
        IsRecording ||
        IsTryingRecognize;

    public bool IsRecording =>
        AudioRecorder.Match(
            ar => ar.IsRecording,
            _ => false);
}
