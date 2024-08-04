using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer;

internal static class Configure
{
    public static IServiceCollection AddSpeechRecognizerEntityServices(this IServiceCollection services) =>
        services
            .AddScoped<SpeechRecognizerStateFacade>();
}
