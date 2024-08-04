using Microsoft.Extensions.Logging;
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
    [Inject] HostInfo _hostInfo { get; init; } = null!;
    [Inject] MauiBlazorCircuitContext _circuitContext { get; init; } = null!;
    [Inject] SvcNavigationManager _navigationManager { get; init; } = null!;
    [Inject] INeedInitializationServicesInitializer _needInitializationServicesInitializer { get; init; } = null!;

    #endregion

    #region Fields

    private MauiWebView? _mauiWebView;

    #endregion

    #region LC Methods

    protected override async Task OnInitializedAsync()
    {
        _mauiWebView = MauiWebView.Current;
        _mauiWebView?.SetScopedServices(_services);
        try
        {
            _circuitContext.RootComponent = this;
            await _needInitializationServicesInitializer.InitializeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "OnInitializedAsync failed, will reload...");
            AppServices.GetRequiredService<MauiReloadUI>().Reload(); // ReloadUI is a singleton on MAUI
        }
    }

    #endregion
}
