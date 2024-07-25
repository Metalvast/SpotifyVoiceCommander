using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store;

internal static class SpeechRecognizerReducers
{
    [ReducerMethod]
    public static SpeechRecognizerState Reduce(
        SpeechRecognizerState state,
        FluxorActionWrapper<StartSpeechRecordingSuccessAction> _) =>
        state with
        {
            IsBusy = true,
        };

    [ReducerMethod]
    public static SpeechRecognizerState Reduce(
        SpeechRecognizerState state,
        FluxorActionWrapper<StopSpeechRecordingSuccessAction> _) =>
        state with
        {
            IsBusy = false,
        };
}
