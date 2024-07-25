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

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

#if IS_DEV_MAUI
        window.Title = "SpotifyVoiceCommander (Dev)";
#else
        window.Title = "SpotifyVoiceCommander";
#endif

        window.MinimumHeight = window.MaximumHeight = window.Height = 600;
        window.MinimumWidth = window.MaximumWidth = window.Width = 400;
        
        return window;
    }
}
