using Microsoft.AspNetCore.Components.WebView;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

public static class BlazorWebViewManagerExt
{
    private static readonly FieldInfo CurrentPageContextField = typeof(WebViewManager)
        .GetField("_currentPageContext", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static IAsyncDisposable? GetCurrentPageContext(this WebViewManager webViewManager) =>
        (IAsyncDisposable?)CurrentPageContextField.GetValue(webViewManager);

    public static void ResetCurrentPageContext(this WebViewManager webViewManager) =>
        CurrentPageContextField.SetValue(webViewManager, null);
}
