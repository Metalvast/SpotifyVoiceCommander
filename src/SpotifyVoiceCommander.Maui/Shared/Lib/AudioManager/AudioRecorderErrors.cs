namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager;

internal static class AudioRecorderErrors
{
    public static Error RestartRequired => Error.Custom(
        2000, 
        "AudioRecorder.RestartRequired",
        "App required restart for AudioRecorder correct work");
}
