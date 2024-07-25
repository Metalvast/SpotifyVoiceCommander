using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Lib;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer;

internal static class Configure
{
    public static IServiceCollection AddSpeechRecognizerServices(this IServiceCollection services) =>
        services.AddScoped<SpeechRecordingManager>();
}
