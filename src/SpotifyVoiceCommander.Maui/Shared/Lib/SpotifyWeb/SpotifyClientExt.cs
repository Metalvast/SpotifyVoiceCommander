using ActualLab.Reflection;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

internal static class SpotifyClientExt
{
    private static readonly Func<SpotifyClient, IAPIConnector> ApiConnectorGetter;

    static SpotifyClientExt()
    {
        var bfInstanceNonPublic = BindingFlags.Instance | BindingFlags.NonPublic;
        var tMudTextField = typeof(SpotifyClient);
        var fIsFocused = tMudTextField.GetField("_apiConnector", bfInstanceNonPublic)!;

        ApiConnectorGetter = fIsFocused.GetGetter<SpotifyClient, IAPIConnector>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IAPIConnector GetApiConnector(this SpotifyClient component) =>
        ApiConnectorGetter(component);


    public static async Task<bool> SafeAddToQueue(
        this IPlayerClient spotifyClient,
        PlayerAddToQueueRequest request,
        CancellationToken cancel = default)
    {
        try
        {
            return await spotifyClient.AddToQueue(request, cancel);
        }
        catch
        {
            return false;
        }
    }

    public static async Task<bool> SafeSkipNext(
        this IPlayerClient spotifyClient,
        PlayerSkipNextRequest request,
        CancellationToken cancel = default)
    {
        try
        {
            return await spotifyClient.SkipNext(request, cancel);
        }
        catch
        {
            return false;
        }
    }
}
