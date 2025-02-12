﻿namespace SpotifyVoiceCommander.Maui.Shared.Api.Lib;

public sealed class InternalApiUrlMapper
{
    public Uri BaseUri { get; }
    public string BaseUrl { get; }
    public Uri ApiBaseUri { get; }
    public string ApiBaseUrl { get; }

    public InternalApiUrlMapper(HostInfo hostInfo)
    {
        if (hostInfo.IsDevelopment)
        {
            BaseUrl = "https://local.spotify-voice-commander.ru/";
            BaseUri = new Uri(BaseUrl);
            ApiBaseUrl = "https://localhost:7193/";
            ApiBaseUri = new Uri(ApiBaseUrl);
            return;
        }

        if (hostInfo.IsProduction)
        {
            BaseUrl = "https://spotify-voice-commander.ru/";
            BaseUri = new Uri(BaseUrl);
            ApiBaseUrl = "https://spotify-voice-commander.ru/";
            ApiBaseUri = new Uri(ApiBaseUrl);
            return;
        }

        throw new InvalidOperationException("HostInfo provider unknown environment");
    }
}
