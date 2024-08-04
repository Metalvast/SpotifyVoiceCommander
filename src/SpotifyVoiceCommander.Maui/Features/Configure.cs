using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer;

namespace SpotifyVoiceCommander.Maui.Features;

internal static class Configure
{
    public static IServiceCollection ConfigureFeatureLayer(this IServiceCollection services) =>
        services
            .AddSpeechRecognizerFeatureServices();
}
