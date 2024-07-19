using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Pages.Viewer;

public partial class SignInPage : SvcComponentFluxorBase
{
    #region Internal events

    private void OnSignInButtonClick() =>
        Dispatch(new SignInAction { });

    #endregion
}
