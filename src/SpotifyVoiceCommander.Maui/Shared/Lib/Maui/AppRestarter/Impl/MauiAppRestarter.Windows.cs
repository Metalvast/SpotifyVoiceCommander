using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.AppRestarter;

partial class MauiAppRestarter(
    ILogger<MauiAppRestarter> _logger,
    SvcNavigationManager _mauiBlazorNavigationManager) 
    : IMauiAppRestarter
{
    public partial async Task Restart(bool startRecognizerImmediately)
    {
        try
        {
            var targetUri = $"spotifyvoicecommanderapp://{_mauiBlazorNavigationManager.GetPlayerUri(startRecognizerImmediately)}";
            await Launcher.TryOpenAsync(targetUri);
            MainThreadExt.InvokeLater(() => Application.Current?.CloseWindow(Application.Current.MainPage!.Window));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Restart or close app failed");
        }
    }
}
