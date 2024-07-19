namespace SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;

public static class AnalyzeSpeechShared
{
    public class CompletionMessageRoles
    {
        public const string System = "system";
        public const string Assistant = "assistant";
        public const string User = "user";
    }
}

public record CompletionOptions
{
    public required bool Stream { get; init; }
    public required double Temperature { get; init; }
    public required int MaxTokens { get; init; }
}

public record CompletionMessage
{
    public required string Role { get; init; }
    public required string Text { get; init; }
}