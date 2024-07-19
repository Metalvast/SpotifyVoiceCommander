using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SpotifyVoiceCommander.Maui.App;

public partial class MauiRouter
{
    #region Params

    [Parameter] public Assembly[] Assemblies { get; set; } = [];
    [Parameter] public Type LayoutComponent { get; set; } = typeof(MainLayout);

    #endregion

    #region Injects

    [Inject] protected IServiceProvider _services { get; init; } = null!;
    [Inject] protected ILogger<MauiRouter> _logger { get; init; } = null!;
    [Inject] protected HostInfo _hostInfo { get; init; } = null!;
    [Inject] protected MauiBlazorCircuitContext _circuitContext { get; init; } = null!;

    #endregion

    #region LC Methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _circuitContext.RootComponent = this;
        //await _needInitializationServicesInitializer.InitializeAsync();
    }

    protected virtual void Dispose(bool disposing) { }

    public void Dispose() =>
        Dispose(true);

    #endregion
}
