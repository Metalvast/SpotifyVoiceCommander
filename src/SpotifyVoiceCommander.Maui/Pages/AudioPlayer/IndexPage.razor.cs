using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Pages.AudioPlayer;

[Route("/")]
[Route($"{Routes.AudioPlayer.BasePath}/{Routes.AudioPlayer.Main}")]
public partial class IndexPage : SvcComponentFluxorBase
{
    private void Start() =>
        Dispatch(new StartSpeechRecordingAction { });
}
