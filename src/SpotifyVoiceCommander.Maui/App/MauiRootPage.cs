using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

namespace SpotifyVoiceCommander.Maui.App;

public partial class MauiRootPage : ContentPage
{
    private static volatile MauiRootPage _current = null!;

    public static MauiRootPage Current => _current;

    public MauiRootPage()
    {
        Interlocked.Exchange(ref _current, this);
        //On<iOS>().SetUseSafeArea(true);
        BackgroundColor = Color.FromArgb("#E9EAED");
        MauiLoadingUI.MarkFirstWebViewCreated();
        RecreateWebView();
    }

    public void RecreateWebView() =>
        Content = new MauiWebView().BlazorWebView;

    public void Reload()
    {
        var mauiWebView = MauiWebView.Current;
        if (mauiWebView == null || mauiWebView.IsDead)
            RecreateWebView();
        else
            mauiWebView.HardNavigateTo(MauiWebView.BaseLocalUri.ToString());
    }
}
