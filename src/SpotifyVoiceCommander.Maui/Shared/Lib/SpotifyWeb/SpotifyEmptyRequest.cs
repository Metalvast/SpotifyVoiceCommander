using SpotifyAPI.Web.Http;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.SpotifyWeb;

public record SpotifyEmptyRequest : IRequest
{
    public static readonly SpotifyEmptyRequest Default = new();

    public Uri BaseAddress => throw new NotImplementedException();
    public Uri Endpoint => throw new NotImplementedException();
    public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();
    public IDictionary<string, string> Parameters => throw new NotImplementedException();
    public HttpMethod Method => throw new NotImplementedException();
    public object? Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}