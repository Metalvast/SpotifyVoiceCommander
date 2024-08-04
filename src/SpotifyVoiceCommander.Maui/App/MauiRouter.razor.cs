using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.App.ServicesAbstractions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.WebView;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.App;

public partial class MauiRouter
{
    #region Params

    [Parameter] public Assembly[] Assemblies { get; set; } = [];
    [Parameter] public Type LayoutComponent { get; set; } = typeof(MainLayout);

    #endregion

    #region Injects

    [Inject] IServiceProvider _services { get; init; } = null!;
    [Inject] ILogger<MauiRouter> _logger { get; init; } = null!;
    [Inject] MauiBlazorCircuitContext _circuitContext { get; init; } = null!;
    [Inject] SvcNavigationManager _navigationManager { get; init; } = null!;
    [Inject] AppScopedServiceStarter _appScopedServiceStarter { get; init; } = null!;

    #endregion

    #region Fields

    private MauiWebView? _mauiWebView;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        _mauiWebView = MauiWebView.Current;
        _mauiWebView?.SetScopedServices(_services);
        try
        {
            _circuitContext.RootComponent = this;
            _ = _appScopedServiceStarter.StartScopedServices();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "OnInitializedAsync failed, will reload...");
            AppServices.GetRequiredService<MauiReloadUI>().Reload(); // ReloadUI is a singleton on MAUI
        }
    }

    #endregion
}
