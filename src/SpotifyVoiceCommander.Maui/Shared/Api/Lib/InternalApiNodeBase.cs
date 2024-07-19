using SpotifyVoiceCommander.Maui.Shared.Api.Shell;

namespace SpotifyVoiceCommander.Maui.Shared.Api.Lib;

public class InternalApiNodeBase(HttpEndpointsClient httpEndpointsClient)
{
    protected readonly HttpEndpointsClient _httpEndpointsClient = httpEndpointsClient;
}
