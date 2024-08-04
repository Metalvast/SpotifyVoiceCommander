using SpotifyVoiceCommander.Maui.Shared.Lib.Maui.AppRestarter;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

namespace SpotifyVoiceCommander.Maui.Pages.AudioPlayer;

[Route("/")]
[Route(SvcRoutes.AudioPlayer.Main)]
public partial class IndexPage : SvcFluxorComponentBase
{
    [SupplyParameterFromQuery] public bool StartRecognizerImmediately { get; set; } 

    [Inject] IMauiAppRestarter _appRestarter { get; init; } = null!;

    private void Start()
    {
        _appRestarter.Restart();

        // Dispatch(new StartSpeechRecordingAction { });
    }
}
