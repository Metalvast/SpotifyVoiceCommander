using System.Text.Json;

namespace SpotifyVoiceCommander.Shared;

public class Constants
{
    public const string SpotifyClientId = "feebcd2647514a1ab786bc7d08a6788a";

    public static readonly JsonSerializerOptions CamelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true,
    };
}
