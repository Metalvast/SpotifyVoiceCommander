using ActualLab.Async;
using CommunityToolkit.Maui.Media;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store;
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

internal class SpeechRecognizerStateFacade(
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

    public string StateText => true switch
    {
        _ when _speechRecognizerState.Value.LastError != default => "Что-то пошло не так...",
        _ when _speechRecognizerState.Value.IsRecording => "Слушаю?",
        _ when _speechRecognizerState.Value.IsTryingRecognize => "Секундочку...",
        //_ when _audioPlayerState.Value.SearchState is LoaderState.Loading => "Выполняю...",
        _ => "",
    };

    private TaskCompletionSource _whenInitialized = TaskCompletionSourceExt.New();
    public Task WhenInitialized => _whenInitialized.Task;   

    public async Task InitializeAsync()
    {
        _tagId = new(nameof(SpeechRecognizerStateFacade));
        _svcFluxorActionResolver.SetSubscriber(this);

        _svcFluxorActionResolver.SubscribeTo<InitializeViewerSuccessAction>()
            .WithHandler(_ => TryStartRecognizerAsync());

        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerAction>();
        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerFailureAction>();
        _svcFluxorActionResolver.SubscribeTo<InitializeSpeechRecognizerSuccessAction>()
            .WithHandler(_ => _whenInitialized.TrySetResult());

        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingAction>();
        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingFailureAction>();
        _svcFluxorActionResolver.SubscribeTo<StartSpeechRecordingSuccessAction>();

        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechAction>();
        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechFailureAction>();
        _svcFluxorActionResolver.SubscribeTo<RecognizeSpeechSuccessAction>();

        await TryStartRecognizerAsync();

        _recognizeStarterService.RecognitionResultUpdated += OnRecognitionResultUpdated;

        if (_speechRecognizerState.Value.InitializingState.IsNoDataState())
            _svcFluxorActionResolver.Dispatch(new InitializeSpeechRecognizerAction { });
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
        if (_speechRecognizerState.Value.InitializingState is not LoaderState.Content ||
            _speechRecognizerState.Value.IsRecording ||
            !e.RecognitionResult.Contains("commander", StringComparison.CurrentCultureIgnoreCase))
            return;

        _svcFluxorActionResolver.Dispatch(new StartSpeechRecordingAction { });
    }

    #endregion

    #region Private

    private Task TryStartRecognizerAsync() =>
        _viewerState.Value.HasAuthenticatedViewer
            ? _recognizeStarterService.StartRecognizerAsync()
            : Task.CompletedTask;

    #endregion
}