using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Lib;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class StartSpeechRecordingEffect(
    IServiceProvider _services,
    SpeechRecordingManager _speechRecordingManager) 
    : BaseEffect<StartSpeechRecordingAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<StartSpeechRecordingAction>> actionWrapper) => actionWrapper
        .ThenDoAsync(aw => _speechRecordingManager.StartAsync(aw.CancellationToken));
}
