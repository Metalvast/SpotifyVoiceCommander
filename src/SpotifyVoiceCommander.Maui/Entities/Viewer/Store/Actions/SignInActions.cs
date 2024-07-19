namespace SpotifyVoiceCommander.Maui.Entities.Viewer.Store.Actions;

public record SignInAction : ISvcAction, IAllowAnonymousAction;
public record SignInFailureAction : ISvcAction, IAllowAnonymousAction;
public record SignInSuccessAction : ISvcAction, IAllowAnonymousAction;

