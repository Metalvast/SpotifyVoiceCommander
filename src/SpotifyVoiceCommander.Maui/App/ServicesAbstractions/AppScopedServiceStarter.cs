using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade;

namespace SpotifyVoiceCommander.Maui.App.ServicesAbstractions;

internal class AppScopedServiceStarter
{
    #region Injects

    private readonly IServiceProvider _services;
    private readonly INeedInitializationServicesInitializer _needInitializationServicesInitializer;
    private readonly Tracer _tracer;

    #endregion

    #region Ctors

    public AppScopedServiceStarter(
        IServiceProvider services,
        INeedInitializationServicesInitializer needInitializationServicesInitializer)
    {
        _services = services;
        _tracer = Tracer.Default;
        _needInitializationServicesInitializer = needInitializationServicesInitializer;
    }

    #endregion

    public Task StartScopedServices() =>
        Task.Run(
            async () =>
            {
                try
                {
                    await Task.WhenAll([
                        _services.GetRequiredService<SpeechRecognizerFacade>().InitializeAsync(),
                        _needInitializationServicesInitializer.InitializeAsync()]);
                }
                catch (Exception e)
                {
                    _tracer.Point($"{nameof(StartScopedServices)} failed, error: " + e);
                }
            },
            CancellationToken.None);
}
