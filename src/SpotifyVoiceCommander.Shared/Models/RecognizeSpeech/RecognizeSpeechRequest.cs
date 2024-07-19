namespace SpotifyVoiceCommander.Shared.Models.RecognizeSpeech;

public record RecognizeSpeechRequest
{
    public required byte[] AudioData { get; init; }
}
