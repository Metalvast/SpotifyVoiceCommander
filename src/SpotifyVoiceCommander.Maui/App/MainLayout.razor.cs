using Microsoft.AspNetCore.Components.Web;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;

namespace SpotifyVoiceCommander.Maui.App;

public partial class MainLayout : EtaiLayoutComponentBase
{
    #region Injects

    [Inject] IMauiAppRestarter _appRestarter { get; init; } = null!;

    #endregion

    #region Fields

    private ErrorBoundary _errorBoundary = null!;

    #endregion

    #region Internal events

    private void OnRestartAppButtonClick()
    {
        _errorBoundary.Recover();
        _appRestarter.Restart(false);
    }

    #endregion
}
