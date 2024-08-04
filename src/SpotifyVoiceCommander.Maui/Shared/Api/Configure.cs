using RestSharp;
using SpotifyVoiceCommander.Maui.Shared.Api.Lib;
using SpotifyVoiceCommander.Maui.Shared.Api.Shell;

namespace SpotifyVoiceCommander.Maui.Shared.Api;

internal static class Configure
{
    public static IServiceCollection AddInternalApi(this IServiceCollection services) =>
        services
            .AddSingleton<InternalApiUrlMapper>()
            .AddScoped<InternalApiAuthenticator>()
            .AddHttpClient(nameof(HttpEndpointsClient), (services, httpClient) =>
            {
                var backendUrlMapper = services.GetRequiredService<InternalApiUrlMapper>();
                httpClient.BaseAddress = new Uri(backendUrlMapper.ApiBaseUrl);
            })
            .Services
            .AddScoped(services =>
            {
                var httpClient = services.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(HttpEndpointsClient));
                return new RestClient(httpClient);
            })
            .AddScoped<HttpEndpointsClient>();
}
