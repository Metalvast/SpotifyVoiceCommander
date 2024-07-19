using SpotifyVoiceCommander.Api.Framework.Authentication;
using SpotifyVoiceCommander.Api.Framework.ReverseProxy;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

_ = bool.TryParse(Environment.GetEnvironmentVariable("SpotifyVoiceCommander_EnableAuthentication"), out var enableAuthentication);

services.Configure<YandexCloudApiSettings>(configuration.GetSection(nameof(YandexCloudApiSettings)));

services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<YandexClustersTransforms>();

if (enableAuthentication)
    services.AddAuthentication(SpotifyAuthenticationOptions.DefaultScheme)
        .AddCustomSpotifyScheme();

var app = builder.Build();

app.UseHttpsRedirection();

if (enableAuthentication)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.Run();
