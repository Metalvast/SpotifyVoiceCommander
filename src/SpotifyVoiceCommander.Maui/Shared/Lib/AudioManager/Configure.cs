using CommunityToolkit.Maui.Media;
using SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager;

internal static class Configure
{
    public static IServiceCollection AddAdditionalAudioServices(this IServiceCollection services) =>
        services
            .AddSingleton(SpeechToText.Default)
            .AddAudioRecorderErrorHandler();
}
