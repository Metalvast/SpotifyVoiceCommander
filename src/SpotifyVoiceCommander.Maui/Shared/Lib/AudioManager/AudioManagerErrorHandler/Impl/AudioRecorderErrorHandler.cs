using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler.Impl;

internal class AudioRecorderErrorHandler(IMauiAppRestarter _appRestarter) : IAudioRecorderErrorHandler
{
    private static readonly Error s_targetError = AudioRecorderErrors.RestartRequired;

    public ErrorOr<IEnumerable<Error>> TryHandleRestartRequiredError(IEnumerable<Error> errors)
    {
        if (errors.Any(error =>
            error.Code == s_targetError.Code &&
            error.NumericType == s_targetError.NumericType))
            _appRestarter.Restart();

        return errors.ToErrorOr();
    }
}
