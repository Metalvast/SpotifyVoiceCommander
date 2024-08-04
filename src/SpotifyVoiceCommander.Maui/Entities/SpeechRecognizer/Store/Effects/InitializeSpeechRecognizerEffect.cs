using CommunityToolkit.Maui.Media;
using Plugin.Maui.Audio;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class InitializeSpeechRecognizerEffect(
    IServiceProvider _services,
    IAudioManager _audioManager)
    : BaseEffect<InitializeSpeechRecognizerAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<InitializeSpeechRecognizerAction>> actionWrapper) => actionWrapper
        .ThenAsync(_ => SpeechToText.Default.RequestPermissions())
        .FailIf(
            isGranted => !isGranted,
            _ => Error.Unexpected())
        .Then(_ => _audioManager.CreateRecorder())
        .FailIf(
            audioRecorder => !audioRecorder.CanRecordAudio,
            _ => Error.Unexpected())
        .Switch(
            audioRecorder => Dispatch(new InitializeSpeechRecognizerSuccessAction
            {
                AudioRecorder = audioRecorder,
            }),
            _ => Dispatch(new InitializeSpeechRecognizerFailureAction { }));
}
