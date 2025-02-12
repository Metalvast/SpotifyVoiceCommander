﻿using ActualLab.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SpotifyVoiceCommander.Maui.Shared.Api.Shell;
using SpotifyVoiceCommander.Maui.Shared.Lib.AuthenticationStateProvider;
using SpotifyVoiceCommander.Maui.Shared.Lib.MudBlazor;
using SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

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



    private SvcAuthenticationStateProvider? c_authenticationStateProvider;
    protected SvcAuthenticationStateProvider _authenticationStateProvider =>
        c_authenticationStateProvider ??= _services.GetRequiredService<SvcAuthenticationStateProvider>();

    private SvcNavigationManager? c_navigationManager;
    protected SvcNavigationManager _navigationManager =>
        c_navigationManager ??= _services.GetRequiredService<SvcNavigationManager>();



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
    protected TagId? CallerId;
    protected CancellationToken CancellationToken;

    #endregion

    #region Public/Protected

    public override async Task HandleAsync(FluxorActionWrapper<TAction> actionWrapper, IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        CallerId = actionWrapper.TagId;
        CancellationToken = actionWrapper.CancellationToken;
        var tracerRegion = Tracer.Default.Region($"{GetType().Name} | {Guid.NewGuid()}", true);
        await InnerHandleAsync(actionWrapper);
        tracerRegion.Close();
    }

    protected ErrorOr<Success> Dispatch<TNextAction>(TNextAction nextAction) where TNextAction : ISvcAction =>
        nextAction.ToErrorOr()
            .Then(action => new FluxorActionWrapper<TNextAction>
            {
                Action = action,
                TagId = CallerId,
                CancellationToken = CancellationToken,
            })
            .ThenDo(_dispatcher.Dispatch)
            .Then(_ => Result.Success);

    public abstract Task InnerHandleAsync(ErrorOr<FluxorActionWrapper<TAction>> actionWrapper);

    protected Error HandleErrorState<TFailureAction>(
        IEnumerable<Error> errors,
        TFailureAction failureAction,
        Error returnError,
        string displayMessage = "Ups, something goes wrong...")
        where TFailureAction : ISvcAction =>
        ShowErrorMessage(errors)
            .ThenDo(_ => Dispatch(failureAction))
            .Then(_ => returnError)
            .Unwrap();

    protected ErrorOr<IEnumerable<Error>> ShowErrorMessage(
        IEnumerable<Error> errorMessages,
        string displayMessage = "Ups, something goes wrong...")
    {
        _logger.LogServerMessage(errorMessages);
        _snackbar.ShowErrorMessage(displayMessage);
        return errorMessages.ToErrorOr();
    }

    #endregion
}
