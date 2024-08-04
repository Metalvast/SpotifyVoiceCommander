using System.Collections.Frozen;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager;

internal class BeepKeys
{
    public static readonly FrozenSet<string> Collection = new List<string>
    {
        Default,
    }.ToFrozenSet();

    public const string Default = "start-recording.wav"; 
}
