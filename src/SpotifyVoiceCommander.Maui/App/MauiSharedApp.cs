namespace SpotifyVoiceCommander.Maui.App;

public class MauiSharedApp : Application
{
    private IServiceProvider Services { get; }

    public MauiSharedApp(MauiRootPage mainPage, IServiceProvider services)
    {
        Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.Application.SetWindowSoftInputModeAdjust(
            this,
            Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);

#if WINDOWS
        // Allows to load mixed content into WebView on Windows
        Environment.SetEnvironmentVariable(
            "WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS",
            "--disable-features=AutoupgradeMixedContent");
#endif

        MainPage = mainPage;
        Services = services;
    }
}
