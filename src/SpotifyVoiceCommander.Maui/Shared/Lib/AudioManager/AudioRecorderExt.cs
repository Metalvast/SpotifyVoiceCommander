using Plugin.Maui.Audio;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager;

internal static class AudioRecorderExt
{
    public static async Task<ErrorOr<IAudioRecorder>> SafeStartAsync(this IAudioRecorder audioRecorder)
    {
        try
        {
            await audioRecorder.StartAsync();
            return audioRecorder.ToErrorOr();
        }
        catch (Exception ex) when (ex.Message.Contains("CO_E_OBJNOTCONNECTED"))
        {
            return AudioRecorderErrors.RestartRequired;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}
