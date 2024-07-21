using SpotifyVoiceCommander.Maui.Entities.Viewer.Models;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store;

[FeatureState]
public record ViewerState
{
    public ViewerModel? Viewer { get; init; }
    public bool IsAdmin { get; init; }
    public LoaderState ViewerLoadingState { get; init; }

    public bool HasAuthenticatedViewer => Viewer != null;
}
