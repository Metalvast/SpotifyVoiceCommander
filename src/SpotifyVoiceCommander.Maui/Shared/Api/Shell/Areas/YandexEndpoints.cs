
using RestSharp;
using SpotifyVoiceCommander.Maui.Shared.Api.Lib;
using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;
using SpotifyVoiceCommander.Shared.Models.RecognizeSpeech;

namespace SpotifyVoiceCommander.Maui.Shared.Api.Shell.Areas;

public class YandexEndpoints(HttpEndpointsClient httpEndpointsClient) : InternalApiNodeBase(httpEndpointsClient)
{
    public Task<ErrorOr<RecognizeSpeechResponse>> RecognizeSpeechAsync(RecognizeSpeechRequest request, CancellationToken ct) =>
        _httpEndpointsClient.SafeExecuteAsync<RecognizeSpeechResponse>(
            new RestRequest("/yandex/speech/v1/stt:recognize", Method.Post)
                .AddQueryParameter("format", "lpcm")
                .AddParameter("application/octet-stream", request.AudioData, ParameterType.RequestBody),
            ct);

    public Task<ErrorOr<AnalyzeSpeechResponse>> AnalyzeSpeechAsync(AnalyzeSpeechRequest request, CancellationToken ct) =>
        _httpEndpointsClient.SafeExecuteAsync<AnalyzeSpeechResponse>(
            new RestRequest("/yandex/foundationModels/v1/completion", Method.Post)
                .AddQueryParameter("recognizeResult", request.RecognizedSpeech),
            ct);
}
