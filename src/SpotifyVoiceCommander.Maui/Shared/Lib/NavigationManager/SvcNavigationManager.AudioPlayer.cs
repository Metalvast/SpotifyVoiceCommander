namespace SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

partial class SvcNavigationManager
{
    public string GetPlayerUri(bool startRecognizerImmediately = false) =>
        Instance.GetUriWithQueryParameters(
            SvcRoutes.AudioPlayer.Main,
            new Dictionary<string, object?>
            {
                ["startRecognizerImmediately"] = startRecognizerImmediately,
            });

    public void NavigateToPlayer(bool startRecognizerImmediately = false) =>
        Instance.NavigateTo(GetPlayerUri(startRecognizerImmediately));
}
