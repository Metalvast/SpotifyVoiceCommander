namespace SpotifyVoiceCommander.Maui.Features.SpeechRecognizer;

internal static class Configure
{
    public static IServiceCollection AddSpeechRecognizerFeatureServices(this IServiceCollection services) =>
        services.AddScoped<SpeechRecognizerFacade.SpeechRecognizerFacade>();
}
