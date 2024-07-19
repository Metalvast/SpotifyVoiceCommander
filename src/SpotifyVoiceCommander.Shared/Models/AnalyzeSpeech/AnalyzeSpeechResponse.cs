namespace SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;

public record AnalyzeSpeechResponse
{
    public required AnalyzeSpeechResult Result { get; init; }
}

public record AnalyzeSpeechResult
{
    public required List<Alternative> Alternatives { get; init; }
    public required Usage Usage { get; init; }
    public required string ModelVersion { get; init; }
}

public record Usage
{
    public required string InputTextTokens { get; init; }
    public required string CompletionTokens { get; init; }
    public required string TotalTokens { get; init; }
}

public record Alternative
{
    public required CompletionMessage Message { get; init; }
    public required string Status { get; init; }
}