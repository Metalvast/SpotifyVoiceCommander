using ActualLab.DependencyInjection;
using ActualLab.Fusion.Client.Caching;
using Microsoft.Extensions.Logging;
using SpotifyVoiceCommander.Maui.App;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

public class MauiReloadUI
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public MauiReloadUI(IServiceProvider services)
    {
        _services = services;
        _logger = services.LogFor(GetType());
    }

    public void Reload(bool clearCaches = false, bool clearLocalSettings = false)
    {
        _logger.LogInformation("Reload requested");
        _ = MainThreadExt.InvokeLaterAsync(async () =>
        {
            _logger.LogInformation("Reloading...");
            try
            {
                await Clear(clearCaches, clearLocalSettings).ConfigureAwait(true);
                MauiRootPage.Current.Reload();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Reload failed, terminating");
                Application.Current?.Quit(); // We can't do much in this case
            }
        });
    }

    public Task Clear(bool clearCaches, bool clearLocalSettings)
    {
        if (!(clearCaches || clearLocalSettings))
            return Task.CompletedTask;

        var clearTasks = new List<Task>();
        if (clearCaches)
            clearTasks.Add(ClearCaches());
        // NOTE(Ivan Stuk): Возможно подобное нужно будет позже
        //if (clearLocalSettings)
        //    clearTasks.Add(ClearLocalSettings());
        return Task.WhenAll(clearTasks);
    }

    public async Task ClearCaches()
    {
        _logger.LogWarning("Cleaning caches...");
        try
        {
            var clientComputedCache = _services.GetService<IClientComputedCache>();
            if (clientComputedCache != null)
                await clientComputedCache.Clear(CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ClearCaches failed");
        }
    }

    public void Quit() =>
        Application.Current?.Quit();
}
