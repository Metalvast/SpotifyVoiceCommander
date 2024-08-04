using Plugin.Maui.Audio;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.BeepManager.Impl;

internal class BeepManager(
    IAudioManager _audioManager)
    : IBeepManager,
    INeedInitialization
{
    #region Fields

    private static readonly Dictionary<string, AsyncAudioPlayer> _beeps = [];
    private bool _initialized;

    #endregion

    #region Initialization

    public bool MustAwait => true;

    public async ValueTask InitializeAsync()
    {
        if (_initialized)
            return;

        _initialized = true;
        var openFileTasks = BeepKeys.Collection.ToDictionary(beepKey => beepKey, FileSystem.OpenAppPackageFileAsync);
        await Task.WhenAll(openFileTasks.Values);
        foreach (var task in openFileTasks)
            _beeps.Add(task.Key, _audioManager.CreateAsyncPlayer(task.Value.Result));
    }

    #endregion

    #region Public methods

    public Task<ErrorOr<Success>> BeepAsync(string beepName = BeepKeys.Default) => beepName.ToErrorOr()
        .Then(beepName => _beeps.GetValueOrDefault(beepName) ?? GetDefaultAudioPlayer())
        .ThenDoAsync(audioPlayer => audioPlayer.PlayAsync(CancellationToken.None))
        .Then(_ => Result.Success);

    #endregion

    #region Private methods

    private ErrorOr<AsyncAudioPlayer> GetDefaultAudioPlayer() =>
        _beeps.TryGetValue(BeepKeys.Default, out var defaultAudioPlayer) 
            ? defaultAudioPlayer 
            : Error.Unexpected();

    #endregion
}
