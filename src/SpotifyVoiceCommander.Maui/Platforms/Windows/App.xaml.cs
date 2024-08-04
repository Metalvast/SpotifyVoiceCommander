// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;

namespace SpotifyVoiceCommander.Maui.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            if (Auth0.OidcClient.Platforms.Windows.Activator.Default.CheckRedirectionActivation())
                return;

            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var actArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            if (actArgs.Kind is ExtendedActivationKind.Protocol &&
                actArgs.Data is IProtocolActivatedEventArgs protocolActivatedEventArgs &&
                !protocolActivatedEventArgs.Uri.PathAndQuery.Contains("callback"))
                _ = WhenScopedServicesReady().ContinueWith(t =>
                {
                    var scopedServices = t.Result;
                    var targetUri = protocolActivatedEventArgs.Uri.ToString().Replace("spotifyvoicecommanderapp://", "");
                    scopedServices.GetRequiredService<NavigationManager>().NavigateTo(targetUri);
                });

            base.OnLaunched(args);
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
