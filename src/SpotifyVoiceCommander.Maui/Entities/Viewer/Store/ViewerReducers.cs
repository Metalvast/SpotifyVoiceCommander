
using SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store;

internal static class ViewerReducers
{
    [ReducerMethod]
    public static ViewerState ReduceInitializeViewerAction(
        ViewerState state,
        FluxorActionWrapper<InitializeViewerAction> _) =>
        state with
        {
            ViewerLoadingState = LoaderState.Loading,
        };

    [ReducerMethod]
    public static ViewerState ReduceInitializeViewerFailureAction(
        ViewerState state,
        FluxorActionWrapper<InitializeViewerFailureAction> _) =>
        state with
        {
            ViewerLoadingState = LoaderState.Error,
        };

    [ReducerMethod]
    public static ViewerState ReduceInitializeViewerSuccessAction(
        ViewerState state,
        FluxorActionWrapper<InitializeViewerSuccessAction> actionWrapper) =>
        state with
        {
            Viewer = actionWrapper.Action.Viewer,
            ViewerLoadingState = LoaderState.Content,
        };
}
