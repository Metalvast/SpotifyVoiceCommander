namespace SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;

public record AnalyzeSpeechRequest
{
    public required string RecognizedSpeech { get; init; }
}