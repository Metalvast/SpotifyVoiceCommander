namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

public record StartSpeechRecordingAction : ISvcAction;
public record StartSpeechRecordingFailureAction : ISvcAction
{
    public required Error Error { get; init; }
}
public record StartSpeechRecordingSuccessAction : ISvcAction;
