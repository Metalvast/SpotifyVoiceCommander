using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Store.Actions;
using SpotifyVoiceCommander.Maui.Shared.Lib.Fluxor.Subscription;

namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Lib;

internal class SpeechRecordingManager(
    IServiceProvider _services,
    IAudioManager _audioManager) 
    : INeedInitialization,
    ISvcFluxorSubscriber,
    IDisposable
{
    #region Fields

    private const int s_recordDurationMs = 5000;
    private SvcFluxorActionResolver _svcFluxorActionResolver = null!;
    private IAudioRecorder _audioRecorder = null!;
    private readonly TagId _tagId = new(nameof(SpeechRecordingManager));

    #endregion

    #region Initialization

    public bool MustAwait => true;

    public ValueTask InitializeAsync()
    {
        _svcFluxorActionResolver = _services.GetRequiredService<SvcFluxorActionResolver>();
        _svcFluxorActionResolver.SetSubscriber(this);
        _audioRecorder ??= _audioManager.CreateRecorder();
        if (_audioRecorder.CanRecordAudio)
            return ValueTask.CompletedTask;

        _svcFluxorActionResolver.Dispatch(new LockRecognizeSpeechAction
        {
            Reason = LockRecognizeSpeechReason.CanNotRecordAudio,
        });

        return ValueTask.CompletedTask;
    }

    #endregion

    #region Public

    public TagId TagId => _tagId;

    public async Task StartAsync(CancellationToken ct = default)
    {
        if (_audioRecorder.IsRecording) 
            return;

        await _audioRecorder.StartAsync();
        _svcFluxorActionResolver.Dispatch(new StartSpeechRecordingSuccessAction { });
        ct.Register(() => _ = StopAsync());
        await Task.Delay(s_recordDurationMs, ct);
        if (ct.IsCancellationRequested)
            return;

        var record = await _audioRecorder.StopAsync();
        var audioData = await GetAudioData(record);
        _svcFluxorActionResolver.Dispatch(new RecognizeSpeechAction
        {
            AudioData = audioData,
        });
    }

    public async Task StopAsync(CancellationToken ct = default)
    {
        if (!_audioRecorder.IsRecording)
            return;

        await _audioRecorder.StopAsync();
    }

    public void Dispose()
    {

    }

    #endregion

    #region Private methods

    private async Task<byte[]> GetAudioData(IAudioSource audioSource)
    {
        using var memoryStream = new MemoryStream();
        using var audioStream = audioSource.GetAudioStream();
        await audioStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    #endregion
}
