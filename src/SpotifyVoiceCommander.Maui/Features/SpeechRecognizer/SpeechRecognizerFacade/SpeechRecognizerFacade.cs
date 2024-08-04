using ActualLab.Async;
using CommunityToolkit.Maui.Media;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade.Models;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

namespace SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade;

internal class SpeechRecognizerFacade(
    IState<ViewerState> _viewerState,
    IState<SpeechRecognizerState> _speechRecognizerState,
    SvcFluxorActionResolver _svcFluxorActionResolver,
    MauiRecognizerStarterService _recognizeStarterService)
    : ISvcFluxorSubscriber,
    IDisposable
{
    #region Public

    private TagId _tagId;
    public TagId TagId => _tagId;

    public event Action<SpeechRecognizerSharedState>? OnStateChanged;

    public SpeechRecognizerSharedState State => true switch
    {
        _ when _speechRecognizerState.Value.LastError != default => SpeechRecognizerSharedState.Error,
        _ when _speechRecognizerState.Value.IsRecording => SpeechRecognizerSharedState.Listening,
        _ when _speechRecognizerState.Value.IsTryingRecognize => SpeechRecognizerSharedState.Proccessing,
        _ => SpeechRecognizerSharedState.Waiting,
    };

    public string StateText => State switch
    {
        SpeechRecognizerSharedState.Error => "Что-то пошло не так...",
        SpeechRecognizerSharedState.Listening => "Слушаю?",
        SpeechRecognizerSharedState.Proccessing => "Секундочку...",
        SpeechRecognizerSharedState.Listening or _ => "Ожидаю...",
    };

    private readonly TaskCompletionSource _whenInitialized = TaskCompletionSourceExt.New();
    public Task WhenInitialized => _whenInitialized.Task;

    public async Task InitializeAsync()
    {
        _tagId = new(nameof(SpeechRecognizerFacade));
        _svcFluxorActionResolver.SetSubscriber(this);

        _svcFluxorActionResolver.SubscribeTo<InitializeViewerSuccessAction>()
            .WithHandler(_ => TryStartRecognizerAsync());

        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerFailureAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerSuccessAction>()
            .WithHandler(_ =>
            {
                _whenInitialized.TrySetResult();
                return TryStartRecognizerAsync();
            })
            .WithHandler(_ => OnStateChanged?.Invoke(State));

        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingFailureAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingSuccessAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));

        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechFailureAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));
        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechSuccessAction>()
            .WithHandler(_ => OnStateChanged?.Invoke(State));

        await TryStartRecognizerAsync();

        _recognizeStarterService.RecognitionResultUpdated += OnRecognitionResultUpdated;

        if (_speechRecognizerState.Value.InitializingState.IsNoDataState())
            _svcFluxorActionResolver.Dispatch(new InitializeSpeechRecognizerAction { });
    }

    public void StartRecording()
    {
        if (_speechRecognizerState.Value.InitializingState is not LoaderState.Content ||
            _speechRecognizerState.Value.IsRecording)
            return;

        _svcFluxorActionResolver.Dispatch(new StartSpeechRecordingAction { });
    }

    public void Dispose()
    {
        _recognizeStarterService.RecognitionResultUpdated -= OnRecognitionResultUpdated;
        _svcFluxorActionResolver.DisposeSilently();
    }

    #endregion

    #region External events

    private void OnRecognitionResultUpdated(SpeechToTextRecognitionResultUpdatedEventArgs e)
    {
        if (!e.RecognitionResult.Contains("commander", StringComparison.CurrentCultureIgnoreCase))
            return;

        StartRecording();
    }

    #endregion

    #region Private

    private Task TryStartRecognizerAsync() =>
        _viewerState.Value.HasAuthenticatedViewer
            ? _recognizeStarterService.StartRecognizerAsync()
            : Task.CompletedTask;

    #endregion
}
