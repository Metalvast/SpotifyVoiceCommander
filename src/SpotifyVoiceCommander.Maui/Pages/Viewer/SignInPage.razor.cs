using Microsoft.AspNetCore.Authorization;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

namespace SpotifyVoiceCommander.Maui.Pages.Viewer;

[AllowAnonymous]
[Route(SvcRoutes.Identity.SignIn)]
public partial class SignInPage : SvcFluxorComponentBase
{
    #region Internal events

    private void OnSignInButtonClick() =>
        Dispatch(new SignInAction { });

    #endregion
}
