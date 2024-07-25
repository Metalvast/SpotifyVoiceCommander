namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

public record LockRecognizeSpeechAction : ISvcAction
{
    public required LockRecognizeSpeechReason Reason { get; init; }
}

public enum LockRecognizeSpeechReason
{
    Default,
    CanNotRecordAudio,
}
