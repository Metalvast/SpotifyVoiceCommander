using CommunityToolkit.Maui.Media;
using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store;
using SpotifyVoiceCommander.Maui.Entities.AudioPlayer.Store.Actions;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Components;

public partial class SpeechRecognizerListner : SvcFluxorComponentBase
{
    #region Params

    [Parameter] public bool StartRecognizerImmediately { get; set; }

    #endregion

    #region Injects

    [Inject] IState<SpeechRecognizerState> _speechRecognizerState { get; init; } = null!;
    [Inject] IState<AudioPlayerState> _audioPlayerState { get; init; } = null!;
    [Inject] MauiRecognizerStarterService _recognizeStarterService { get; init; } = null!;

    #endregion

    #region Fields

    private ActionState _recordingStartActionState;

    private string _stateText => true switch
    {
        _ when
            _speechRecognizerState.Value.LastError != default ||
            _audioPlayerState.Value.SearchState is LoaderState.Error => "Что-то пошло не так...",
        _ when _speechRecognizerState.Value.IsRecording => "Слушаю?",
        _ when _speechRecognizerState.Value.IsTryingRecognize => "Секундочку...",
        _ when _audioPlayerState.Value.SearchState is LoaderState.Loading => "Выполняю...",
        _ => "",
    };

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeTo<InitializeSpeechRecognizerAction>();
        SubscribeTo<InitializeSpeechRecognizerFailureAction>();
        SubscribeTo<InitializeSpeechRecognizerSuccessAction>()
            .WithHandler(_ =>
            {
                if (!StartRecognizerImmediately)
                    return;

                DispatchStartRecordingAction();
            });

        SubscribeTo<StartSpeechRecordingAction>()
            .WithHandler(_ => _recordingStartActionState = ActionState.InProgress);
        SubscribeTo<StartSpeechRecordingFailureAction>()
            .WithHandler(_ => _recordingStartActionState = ActionState.WaitingStart);
        SubscribeTo<StartSpeechRecordingSuccessAction>()
            .WithHandler(_ => _recordingStartActionState = ActionState.WaitingStart);

        SubscribeTo<RecognizeSpeechAction>();
        SubscribeTo<RecognizeSpeechFailureAction>();
        SubscribeTo<RecognizeSpeechSuccessAction>();

        SubscribeTo<FindAndPlayTrackAction>();
        SubscribeTo<FindAndPlayTrackFailureAction>();
        SubscribeTo<FindAndPlayTrackSuccessAction>();

        _recognizeStarterService.RecognitionResultUpdated += OnRecognitionResultUpdated;

        if (_speechRecognizerState.Value.InitializingState.IsNoDataState())
            Dispatch(new InitializeSpeechRecognizerAction { });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _recognizeStarterService.RecognitionResultUpdated -= OnRecognitionResultUpdated;
        }
    }

    #endregion

    #region External events

    private void OnRecognitionResultUpdated(SpeechToTextRecognitionResultUpdatedEventArgs e)
    {
        if (_recordingStartActionState is not ActionState.WaitingStart ||
            _speechRecognizerState.Value.IsRecording ||
            !e.RecognitionResult.Contains("commander", StringComparison.CurrentCultureIgnoreCase))
            return;

        DispatchStartRecordingAction();
    }

    private void DispatchStartRecordingAction()
    {
        _recordingStartActionState = ActionState.InProgress;
        Dispatch(new StartSpeechRecordingAction { });
    }

    #endregion
}
