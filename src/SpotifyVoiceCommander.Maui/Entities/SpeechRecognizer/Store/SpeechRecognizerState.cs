namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

[FeatureState]
public record SpeechRecognizerState
{
    public bool IsBusy { get; init; }
}
