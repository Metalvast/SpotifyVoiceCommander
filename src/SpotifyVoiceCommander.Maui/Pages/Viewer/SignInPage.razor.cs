using Microsoft.AspNetCore.Authorization;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Pages.Viewer;

[AllowAnonymous]
[Route($"{Routes.Identity.BasePath}/{Routes.Identity.SignIn}")]
public partial class SignInPage : SvcComponentFluxorBase
{
    #region Internal events

    private void OnSignInButtonClick() =>
        Dispatch(new SignInAction { });

    #endregion
}
