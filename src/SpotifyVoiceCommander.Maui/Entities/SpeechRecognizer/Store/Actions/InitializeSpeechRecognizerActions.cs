using Plugin.Maui.Audio;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

public record InitializeSpeechRecognizerAction : ISvcAction;
public record InitializeSpeechRecognizerFailureAction : ISvcAction;
public record InitializeSpeechRecognizerSuccessAction : ISvcAction
{
    public required IAudioRecorder AudioRecorder { get; init; }
}
