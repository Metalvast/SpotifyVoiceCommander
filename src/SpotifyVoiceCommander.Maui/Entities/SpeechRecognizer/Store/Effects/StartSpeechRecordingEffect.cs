using Plugin.Maui.Audio;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.AudioManager.AudioManagerErrorHandler;
using SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager;
using SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class StartSpeechRecordingEffect(
    IServiceProvider _services,
    IState<SpeechRecognizerState> _speechRecognizerState,
    MauiRecognizerStarterService _recognizeStarterService,
    IBeepManager _beepManager,
    IAudioRecorderErrorHandler _audioRecorderErrorHandler)
    : BaseEffect<StartSpeechRecordingAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<StartSpeechRecordingAction>> actionWrapper) => actionWrapper
        .FailIf(
            _ => _speechRecognizerState.Value.IsBusy,
            _ => Error.Conflict())
        .Then(_ => _speechRecognizerState.Value.AudioRecorder)
        .ThenDoAsync(_ => _recognizeStarterService.Lock())
        .ThenAsync(audioRecorder => audioRecorder.SafeStartAsync())
        .Else(errors =>
            Dispatch(new StartSpeechRecordingFailureAction
            {
                Error = errors.First(),
            })
            .Then(_ => errors.First())
            .Unwrap())
        .ThenDo(_ => _beepManager.BeepAsync())
        .ThenDo(_ => Dispatch(new StartSpeechRecordingSuccessAction { }))
        .ThenDo(audioRecorder => CancellationToken.Register(() => _ = audioRecorder.StopAsync()))
        .ThenDoAsync(_ => Shared.Lib.System.TaskExt.SafeDelay(5500, CancellationToken))
        .FailIf(
            _ => CancellationToken.IsCancellationRequested,
            _ => Error.Unexpected())
        .ThenAsync(audioRecorder => audioRecorder.StopAsync())
        .ThenDo(_ => _beepManager.BeepAsync())
        .ThenAsync(GetAudioData)
        .SwitchAsync(
            audioData =>
                Dispatch(new RecognizeSpeechAction
                {
                    AudioData = audioData,
                })
                .ThenDoAsync(_ => _recognizeStarterService.Unlock()),
            errors => _audioRecorderErrorHandler.TryHandleRestartRequiredError(errors)
                .ThenDoAsync(_ => _recognizeStarterService.Unlock()));

    private async Task<byte[]> GetAudioData(IAudioSource audioSource)
    {
        using var memoryStream = new MemoryStream();
        using var audioStream = audioSource.GetAudioStream();
        await audioStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
