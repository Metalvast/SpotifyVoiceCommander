using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer;

namespace SpotifyVoiceCommander.Maui.Entities;

internal static class Configure
{
    public static IServiceCollection ConfigureEntityLayer(this IServiceCollection services) =>
        services
            .AddSpeechRecognizerEntityServices();
}
