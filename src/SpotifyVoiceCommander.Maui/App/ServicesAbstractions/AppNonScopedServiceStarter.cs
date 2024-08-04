using ActualLab.DependencyInjection;

namespace SpotifyVoiceCommander.Maui.App.ServicesAbstractions;

internal class AppNonScopedServiceStarter
{
    #region Injects

    private readonly IServiceProvider _services;
    private readonly Tracer _tracer;

    #endregion

    #region Ctors

    public AppNonScopedServiceStarter(IServiceProvider services)
    {
        _services = services;
        _tracer = Tracer.Default;
    }

    #endregion

    public Task StartNonScopedServices() =>
        Task.Run(
            async () =>
            {
                try
                {
                    await StartHostedServices();
                }
                catch (Exception e)
                {
                    _tracer.Point($"{nameof(StartNonScopedServices)} failed, error: " + e);
                }
            },
            CancellationToken.None);

    private async Task StartHostedServices()
    {
        using var _ = _tracer.Region();
        var tasks = new List<Task>();
        var tracePrefix = nameof(StartHostedServices) + ": starting ";
        foreach (var hostedService in _services.HostedServices())
        {
            _tracer.Point(tracePrefix + hostedService.GetType().Name);
            tasks.Add(hostedService.StartAsync(default));
        }
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
