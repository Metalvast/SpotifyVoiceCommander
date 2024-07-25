using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Lib;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Effects;

internal class StopSpeechRecordingEffect(
    IServiceProvider _services,
    SpeechRecordingManager _speechRecordingManager)
    : BaseEffect<StopSpeechRecordingAction>(_services)
{
    public override Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<StopSpeechRecordingAction>> actionWrapper) => actionWrapper
        .ThenDoAsync(aw => _speechRecordingManager.StopAsync(aw.CancellationToken));
}
