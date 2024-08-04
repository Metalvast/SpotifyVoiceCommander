using Microsoft.JSInterop;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

public partial class SvcNavigationManager(
    BlazorNavigationManager _navigationManager,
    IJSRuntime _jsRuntime)
{
    #region Public

    public BlazorNavigationManager Instance =>
        _navigationManager;

    public string RelativeUri =>
        ToRelativeUri(Instance.Uri);

    public string ToRelativeUri(string uri) =>
        uri.Replace(Instance.BaseUri, null);

    public ValueTask ChangeHistoryWithoutNavigate(string uri) =>
        _jsRuntime
            .InvokeVoidAsync("eval", $"window.history.pushState({{}}, '', '{uri}')");

    #endregion
}
