using System.Collections.Immutable;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor;

public interface ISvcAction { }
public interface IAllowAnonymousAction { }
public interface IHasErrorsAction
{
    ImmutableList<string> Errors { get; init; }
}