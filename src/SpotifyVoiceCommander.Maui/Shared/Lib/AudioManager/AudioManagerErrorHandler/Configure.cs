using SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler.Impl;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler;

internal static class Configure
{
    public static IServiceCollection AddAudioRecorderErrorHandler(this IServiceCollection services) =>
        services.AddSingleton<IAudioRecorderErrorHandler, AudioRecorderErrorHandler>();
}
