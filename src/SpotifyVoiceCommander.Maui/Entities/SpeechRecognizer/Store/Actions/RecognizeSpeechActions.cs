using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

public record RecognizeSpeechAction : ISvcAction
{
    public required byte[] AudioData { get; init; }
}
public record RecognizeSpeechFailureAction : ISvcAction;
public record RecognizeSpeechSuccessAction : ISvcAction
{
    public required AnalyzeSpeechResult Result { get; init; }
}