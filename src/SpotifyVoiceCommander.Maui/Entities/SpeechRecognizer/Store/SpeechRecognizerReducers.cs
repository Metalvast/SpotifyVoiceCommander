using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

internal static class SpeechRecognizerReducers
{
    [ReducerMethod]
    public static SpeechRecognizerState ReduceInitializeSpeechRecognizerAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<InitializeSpeechRecognizerAction> _) =>
        state with
        {
            InitializingState = LoaderState.Loading,
            AudioRecorder = SvcErrors.NotInitialized,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceInitializeSpeechRecognizerFailureAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<InitializeSpeechRecognizerFailureAction> _) =>
        state with
        {
            InitializingState = LoaderState.Error,
            AudioRecorder = SvcErrors.InitializationFailed,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceInitializeSpeechRecognizerSuccessAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<InitializeSpeechRecognizerSuccessAction> actionWrapper) =>
        state with
        {
            InitializingState = LoaderState.Content,
            AudioRecorder = actionWrapper.Action.AudioRecorder.ToErrorOr(),
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceStartSpeechRecordingAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<StartSpeechRecordingAction> _) =>
        state with
        {
            LastError = default,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceStartSpeechRecordingFailureAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<StartSpeechRecordingFailureAction> actionWrapper) =>
        state with
        {
            LastError = actionWrapper.Action.Error,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceStartSpeechRecordingSuccessAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<StartSpeechRecordingSuccessAction> _) =>
        state with
        {
            IsTryingRecognize = true,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceRecognizeSpeechFailureAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<RecognizeSpeechFailureAction> _) =>
        state with
        {
            IsTryingRecognize = false,
        };

    [ReducerMethod]
    public static SpeechRecognizerState ReduceRecognizeSpeechSuccessAction(
        SpeechRecognizerState state,
        FluxorActionWrapper<RecognizeSpeechSuccessAction> _) =>
        state with
        {
            IsTryingRecognize = false,
        };
}
