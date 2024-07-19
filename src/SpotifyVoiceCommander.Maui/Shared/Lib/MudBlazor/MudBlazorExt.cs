using Microsoft.Extensions.Logging;
using MudBlazor;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.MudBlazor;

internal static class MudBlazorExt
{
    public static Snackbar ShowErrorMessage(
        this ISnackbar snackbar,
        string errorMessage = "Ups, something goes wrong...") =>
        snackbar.Add(errorMessage, Severity.Error);

    public static void LogServerMessage(this ILogger logger, IEnumerable<Error> errorMessages)
    {
        if (!errorMessages.Any())
            return;

        logger.LogWarning("{messages}", string.Join("\n", errorMessages.Select(x => x.Description)));
    }
}
