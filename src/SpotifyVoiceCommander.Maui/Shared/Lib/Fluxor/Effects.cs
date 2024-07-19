using ActualLab.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SpotifyVoiceCommander.Maui.Shared.Api.Shell;
using SpotifyVoiceCommander.Maui.Shared.Framework;
using SpotifyVoiceCommander.Maui.Shared.Lib.MudBlazor;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor;

public abstract class BaseEffect<TAction> : Effect<FluxorActionWrapper<TAction>>
    where TAction : ISvcAction
{
    #region Injects

    protected readonly IServiceProvider _services;

    #endregion

    #region Ctors

    protected BaseEffect(IServiceProvider services)
    {
        _services = services;
    }

    #endregion

    #region Services 

    private ILogger? c_logger;
    protected ILogger _logger =>
        c_logger ??= _services.LogFor(GetType());



    private MauiAuthenticationStateProvider? c_authenticationStateProvider;
    protected MauiAuthenticationStateProvider _authenticationStateProvider =>
        c_authenticationStateProvider ??= _services.GetRequiredService<MauiAuthenticationStateProvider>();

    private MauiBlazorNavigationManager? c_navigationManager;
    protected MauiBlazorNavigationManager _navigationManager =>
        c_navigationManager ??= _services.GetRequiredService<MauiBlazorNavigationManager>();



    private HttpEndpointsClient? c_httpEndpointsClient;
    protected HttpEndpointsClient _httpEndpointsClient =>
        c_httpEndpointsClient ??= _services.GetRequiredService<HttpEndpointsClient>();



    private IDialogService? c_dialogService;
    protected IDialogService _dialogService =>
        c_dialogService ??= _services.GetRequiredService<IDialogService>();

    private ISnackbar? c_snackbar;
    protected ISnackbar _snackbar =>
        c_snackbar ??= _services.GetRequiredService<ISnackbar>();

    #endregion

    #region Fields

    private IDispatcher _dispatcher = null!;
    private TagId? _callerId;
    private CancellationToken _cancellationToken;

    #endregion

    #region Public/Protected

    public override async Task HandleAsync(FluxorActionWrapper<TAction> actionWrapper, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _callerId = actionWrapper.TagId;
        _cancellationToken = actionWrapper.CancellationToken;
        var tracerRegion = Tracer.Default.Region($"{GetType().Name} | {Guid.NewGuid()}", true);
        await InnerHandleAsync(actionWrapper);
        tracerRegion.Close();
    }

    protected ErrorOr<Success> Dispatch<TNextAction>(TNextAction nextAction) where TNextAction : ISvcAction =>
        nextAction.ToErrorOr()
            .Then(action => new FluxorActionWrapper<TNextAction>
            {
                Action = action,
                TagId = _callerId,
                CancellationToken = _cancellationToken,
            })
            .ThenDo(_dispatcher.Dispatch)
            .Then(_ => Result.Success);

    public abstract Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<TAction>> actionWrapper);

    public ErrorOr<IEnumerable<Error>> ShowErrorMessage(
        IEnumerable<Error> errorMessages,
        string displayMessage = "Ups, something goes wrong...")
    {
        _logger.LogServerMessage(errorMessages);
        _snackbar.ShowErrorMessage(displayMessage);
        return errorMessages.ToErrorOr();
    }

    #endregion
}
