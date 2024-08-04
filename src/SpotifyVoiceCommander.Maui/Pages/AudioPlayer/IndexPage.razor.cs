using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

namespace SpotifyVoiceCommander.Maui.Pages.AudioPlayer;

[Route("/")]
[Route(SvcRoutes.AudioPlayer.Main)]
public partial class IndexPage : SvcFluxorComponentBase
{
    #region Params

    [SupplyParameterFromQuery] public bool StartRecognizerImmediately { get; set; }

    #endregion

    #region Injects

    [Inject] SpeechRecognizerFacade _speechRecognizerFacade { get; init; } = null!;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitializedAsync();

        if (StartRecognizerImmediately)
            _ = _speechRecognizerFacade.WhenInitialized.ContinueWith(_ => Dispatch(new StartSpeechRecordingAction { }));
    }

    #endregion
}
