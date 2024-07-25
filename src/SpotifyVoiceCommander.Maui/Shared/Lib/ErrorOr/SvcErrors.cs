namespace SpotifyVoiceCommander.Maui.Shared.Lib.ErrorOr;

internal static class SvcErrors
{
    public static readonly Error NotInitialized = Error.Custom(600, "Global.NotInitialized", "Not initialized");
    public static readonly Error InitializationFailed = Error.Custom(601, "Global.InitializationFailed", "Initialization failed");
}
