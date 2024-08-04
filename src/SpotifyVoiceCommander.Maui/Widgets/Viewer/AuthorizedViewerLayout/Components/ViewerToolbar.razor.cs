using SpotifyVoiceCommander.Maui.Entities.Viewer.Store;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Widgets.Viewer.AuthorizedViewerLayout.Components;

public partial class ViewerToolbar : SvcFluxorComponentBase
{
    #region Injects

    [Inject] IState<ViewerState> _viewerState { get; init; } = null!;

    #endregion

    #region Fields

    private string _name => 
        _viewerState.Value.Viewer?.Name ?? "-";

    #endregion

    #region Internal events

    private void OnSignOutButtonClick() =>
        Dispatch(new SignOutAction { });

    #endregion
}
