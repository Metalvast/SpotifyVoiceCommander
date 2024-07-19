using System.Reflection;

namespace SpotifyVoiceCommander.Maui.App;

internal static class MauiSettings
{
    public const string Environment = "Development";
    public const string Host = "local.etime.su";
#if DEVELOPMENT
#elif DEVELOPMENT_STAND
    public const string Environment = "DevelopmentStand";
    public const string Host = "appdev.etime.su";
#elif STAGING
    public const string Environment = "Staging";
    public const string Host = "appstage.etime.su";
#elif PRODUCTION
    public const string Environment = "Production";
    public const string Host = "app.etime.su";
#endif

    internal const string LocalHost = "0.0.0.0";
    internal static readonly Uri BaseUri;
    internal static readonly string BaseUrl;

    internal static readonly Assembly MainAssembly = typeof(MauiSettings).Assembly;

    internal static readonly Assembly[] TargetAssemblies =
    [

    ];

    internal static readonly Assembly[] TargetUIAssemblies =
    [
        MainAssembly,
    ];

    internal static readonly Dictionary<string, object?> ClientAppParameters = null!;

    static MauiSettings()
    {
        BaseUrl = "https://" + Host + "/";
        BaseUri = new Uri(BaseUrl);

        ClientAppParameters = new()
        {
            [nameof(MauiRouter.Assemblies)] = TargetUIAssemblies,
        };
    }
}
