using ActualLab.DependencyInjection;
using JsonRepairUtils;
using Microsoft.Extensions.Options;
using SpotifyVoiceCommander.Api.Framework.ReverseProxy.Models;
using SpotifyVoiceCommander.Shared;
using SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApplication1.Models;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;
using static SpotifyVoiceCommander.Shared.Models.AnalyzeSpeech.AnalyzeSpeechShared;

namespace SpotifyVoiceCommander.Api.Framework.ReverseProxy;

public class YandexClustersTransforms : ITransformProvider
{
    public void Apply(TransformBuilderContext context)
    {
        if (context.Cluster == null || !context.Cluster.ClusterId.StartsWith("Yandex"))
            return;

        var logger = context.Services.LogFor(GetType());

        var yandexCloudApiSettings = context.Services.GetRequiredService<IOptions<YandexCloudApiSettings>>().Value;

        context.AddRequestTransform(transformContext =>
        {
            transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Api-Key", yandexCloudApiSettings.ApiKey);
            return ValueTask.CompletedTask;
        });

        if (context.Cluster.ClusterId == "YandexSpeechAnalyzeCluster")
        {
            context.AddRequestTransform(transformContext =>
            {
                if (!transformContext.Query.Collection.TryGetValue("recognizeResult", out var recognizeResult))
                    return ValueTask.CompletedTask;

                transformContext.ProxyRequest.Content = JsonContent.Create(new AnalyzeSpeechRequestProxyBody
                {
                    ModelUri = $"gpt://{yandexCloudApiSettings.FolderId}/yandexgpt/latest",
                    CompletionOptions = new()
                    {
                        MaxTokens = 500,
                        Stream = true,
                        Temperature = 0.3,
                    },
                    Messages =
                    [
                        new CompletionMessage
                        {
                            Role = CompletionMessageRoles.System,
                            Text = yandexCloudApiSettings.Prompt,
                        },
                        new CompletionMessage
                        {
                            Role = CompletionMessageRoles.User,
                            Text = recognizeResult.ToString(),
                        },
                    ]
                });

                return ValueTask.CompletedTask;
            });

            context.AddResponseTransform(async transformContext =>
            {
                // Enable throwing exceptions when JSON code can not be repaired or even understood (enabled by default)
                var JsonRepair = new JsonRepair
                {
                    ThrowExceptions = true
                };

                try
                {
                    var responseContent = await transformContext.ProxyResponse!.Content.ReadAsStringAsync();
                    string repaired = JsonRepair.Repair(responseContent);
                    var results = JsonSerializer.Deserialize<AnalyzeSpeechResponse[]>(repaired, Constants.CamelCaseJsonSerializerOptions)!;
                    transformContext.ProxyResponse.Content = JsonContent.Create(results[1]);
                }
                catch (JsonRepairError jsonRepairError)
                {
                    logger.LogError("Error message: {Message}. Position: {Position}",
                        jsonRepairError.Message,
                        jsonRepairError.Data["Position"]);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while trying check/repair response");
                }
            });
        }
    }

    public void ValidateCluster(TransformClusterValidationContext context) { }
    public void ValidateRoute(TransformRouteValidationContext context) { }
}
