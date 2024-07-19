using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Etai;

internal sealed class MauiCurrentWebViewContextProvider : ICurrentWebViewContextProvider
{
    public object? GetWebView() =>
        MauiWebView.Current?.PlatformWebView;

    public bool TryGetScopedServices([NotNullWhen(true)] out IServiceProvider? scopedServices) =>
        TryGetScopedServices(out scopedServices);
}
