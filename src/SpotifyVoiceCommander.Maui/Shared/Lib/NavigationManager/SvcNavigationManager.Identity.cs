namespace SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

partial class SvcNavigationManager
{
    public string GetSignInUri(string? redirectUri = null) =>
        Instance.GetUriWithQueryParameters(
            SvcRoutes.Identity.SignIn,
            new Dictionary<string, object?>
            {
                ["RedirectUri"] = redirectUri
            });

    public void NavigateToSignIn(
        string? redirectUri = null,
        bool forceReload = false) =>
        Instance.NavigateTo(
            GetSignInUri(redirectUri),
            forceReload);
}
