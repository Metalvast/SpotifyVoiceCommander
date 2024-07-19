using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SpotifyVoiceCommander.Maui.Shared.Framework;

public class MauiBlazorNavigationManager(
    NavigationManager _navigationManager,
    IJSRuntime _jsRuntime)
{
    #region Public

    public NavigationManager Instance =>
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
