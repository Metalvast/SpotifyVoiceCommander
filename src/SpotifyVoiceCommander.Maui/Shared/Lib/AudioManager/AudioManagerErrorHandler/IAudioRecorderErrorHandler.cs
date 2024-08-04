namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler;

internal interface IAudioRecorderErrorHandler
{
    ErrorOr<IEnumerable<Error>> TryHandleRestartRequiredError(IEnumerable<Error> errors);
}