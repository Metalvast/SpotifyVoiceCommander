using CommunityToolkit.Maui.Media;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.Maui;

internal class MauiRecognizerStarterService(
    ILogger<MauiRecognizerStarterService> _logger,
    ISpeechToText _speechToText)
    : IHostedService
{
    #region LC Methods

    public Task StartAsync(CancellationToken _)
    {
        _speechToText.StateChanged += OnSpeechToTextStateChanged;
        _speechToText.RecognitionResultUpdated += OnRecognitionResultUpdated;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken _)
    {
        _speechToText.StateChanged -= OnSpeechToTextStateChanged;
        _speechToText.RecognitionResultUpdated -= OnRecognitionResultUpdated;
        return _speechToText.DisposeSilentlyAsync().AsTask();
    }

    #endregion

    #region External events

    private void OnSpeechToTextStateChanged(object? sender, SpeechToTextStateChangedEventArgs args)
    {
        _logger.LogDebug("Recognize starter state: {State}", args.State);

        if (args.State is SpeechToTextState.Listening)
            return;

        _ = StartRecognizerAsync();
    }

    private void OnRecognitionResultUpdated(object? _, SpeechToTextRecognitionResultUpdatedEventArgs args)
    {
        _logger.LogDebug("Recognize starter value: {Value}", args.RecognitionResult);

        RecognitionResultUpdated?.Invoke(args);
    }

    #endregion

    #region Public

    public event Action<SpeechToTextRecognitionResultUpdatedEventArgs>? RecognitionResultUpdated;

    public Task StartRecognizerAsync() =>
        _speechToText.StartListenAsync(CultureInfo.GetCultureInfo("en-US"), CancellationToken.None);

    public Task Lock()
    {
        _logger.LogDebug("Maui recognizer locked");
        return _speechToText.StopListenAsync();
    }

    public Task Unlock()
    {
        _logger.LogDebug("Maui recognizer unlocked");
        return StartRecognizerAsync();
    }

    #endregion
}
