namespace SpotifyVoiceCommander.Maui.Shared.Framework;

internal static class MauiBlazorNavigationManagerExt
{
    public static string GetSignInUri(
        this MauiBlazorNavigationManager navigationManager,
        string? redirectUri = null) =>
        navigationManager.Instance.GetUriWithQueryParameters(
            $"{Routes.Identity.BasePath}/{Routes.Identity.SignIn}",
            new Dictionary<string, object?> { ["RedirectUri"] = redirectUri });

    public static void NavigateToPlayer(this MauiBlazorNavigationManager navigationManager) =>
        navigationManager.Instance.NavigateTo("/");
}
