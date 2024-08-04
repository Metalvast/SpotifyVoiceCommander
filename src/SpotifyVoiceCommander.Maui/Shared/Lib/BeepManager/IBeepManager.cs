namespace SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager;

internal interface IBeepManager
{
    Task<ErrorOr<Success>> BeepAsync(string beepName = BeepKeys.Default);
}
