using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade;
using SpotifyVoiceCommander.Maui.Features.SpeechRecognizer.SpeechRecognizerFacade.Models;

namespace SpotifyVoiceCommander.Maui.Widgets.Viewer.AuthorizedViewerLayout.Components;

public partial class SpeechRecognizerToolbar : SvcFluxorComponentBase
{
    #region Injects

    [Inject] SpeechRecognizerFacade _speechRecognizerStateFacade { get; init; } = null!;

    #endregion

    #region Fields

    private string _stateText =>
        _speechRecognizerStateFacade.StateText;

    public Color _stateIconColor => _speechRecognizerStateFacade.State switch
    {
        SpeechRecognizerSharedState.Listening => Color.Info,
        SpeechRecognizerSharedState.Proccessing => Color.Warning,
        SpeechRecognizerSharedState.Error => Color.Error,
        SpeechRecognizerSharedState.Waiting or _ => Color.Success,
    };

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _speechRecognizerStateFacade.OnStateChanged += OnSpeechRecognizerStateChanged;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _speechRecognizerStateFacade.OnStateChanged -= OnSpeechRecognizerStateChanged;
    }

    #endregion

    #region External events

    private void OnSpeechRecognizerStateChanged(SpeechRecognizerSharedState _) =>
        CallStateHasChanged();

    #endregion

    #region Internal events

    private void OnStartRecordingCommandButtonClick() =>
        _speechRecognizerStateFacade.StartRecording();

    #endregion
}
