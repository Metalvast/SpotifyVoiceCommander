namespace SpotifyVoiceCommander.Maui.Shared.Lib.System;

internal static class TaskExt
{
    public static async Task SafeDelay(int millisecondsDelay, CancellationToken ct)
    {
        try
        {
            await Task.Delay(millisecondsDelay, ct);
        }
        catch
        {
            // ignore
        }
    }
}
