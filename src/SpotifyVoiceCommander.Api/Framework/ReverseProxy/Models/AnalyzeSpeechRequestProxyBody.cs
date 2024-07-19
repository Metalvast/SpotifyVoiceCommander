using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;

namespace SpotifyVoiceCommander.Api.Framework.ReverseProxy.Models;

public record AnalyzeSpeechRequestProxyBody
{
    public required string ModelUri { get; init; }
    public required CompletionOptions CompletionOptions { get; init; }
    public required List<CompletionMessage> Messages { get; init; }
}
