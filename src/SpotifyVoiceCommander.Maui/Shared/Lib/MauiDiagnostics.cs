using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using SpotifyVoiceCommander.Maui.Shared.Helpers;
using ILogger = Serilog.ILogger;

namespace SpotifyVoiceCommander.Maui.Shared.Lib;

public static class MauiDiagnostics
{
    public const string LogFolder = "Logs";
    public const string LogFile = "eTime.log";
    public const string AndroidOutputTemplate = "({ThreadID}) [{SourceContext}] {Message:l}{NewLine:l}{Exception}";
    public const string OutputTemplate = "{Timestamp:HH:mm:ss.fff} {Level:u3} T{ThreadID} [{SourceContext}] {Message:l}{NewLine}{Exception}";
    public const string DebugOutputTemplate = "{Timestamp:mm:ss.fff} {Level:u3} T{ThreadID} [{SourceContext}] {Message:l}{NewLine}{Exception}";
    public const long FileSizeLimit = 10_000_000L;

#if DEVELOPMENT
    public const string LogTag = "localhost";
#elif DEVELOPMENT_STAND
    public const string LogTag = "appdev.etime.su";
#elif STAGING
    public const string LogTag = "appstage.etime.su";
#elif PRODUCTION
    public const string LogTag = "app.etime.su";
#else
    public const string LogTag = "etai";
#endif

    public static readonly ILoggerFactory LoggerFactory;
    public static readonly Tracer Tracer;

    public static void Init() { }

    static MauiDiagnostics()
    {
        Log.Logger = CreateAppLogger();
        LoggerFactory = new SerilogLoggerFactory(Log.Logger);
        Tracer.Default = Tracer = CreateAppTracer();
    }

    private static ILogger CreateAppLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.With(new ThreadIdEnricher())
            .Enrich.FromLogContext()
            .Enrich.WithProperty(Serilog.Core.Constants.SourceContextPropertyName, "app.maui")
            .AddPlatformLoggerSinks()
            .CreateLogger();

    private static Tracer CreateAppTracer()
    {
#if DEBUG
        var logger = Log.Logger.ForContext(Serilog.Core.Constants.SourceContextPropertyName, "@trace");
        return new Tracer("MauiApp", x => logger.Debug(x.Format()));
#else
        return Tracer.None;
#endif
    }

    private static LoggerConfiguration AddPlatformLoggerSinks(this LoggerConfiguration logging)
    {
#if WINDOWS
        var logPath = Path.Combine(FileSystem.AppDataDirectory, LogFolder, LogFile);
        logging = logging.WriteTo.Debug(outputTemplate: DebugOutputTemplate);
        logging = logging.WriteTo.File(logPath,
            outputTemplate: OutputTemplate,
            fileSizeLimitBytes: FileSizeLimit);
#elif ANDROID
        logging = logging.WriteTo.AndroidTaggedLog(LogTag, outputTemplate: AndroidOutputTemplate);
#endif
        return logging;
    }


    public static ILoggingBuilder AddFilteringSerilog(
        this ILoggingBuilder builder,
        ILogger? logger = null,
        bool dispose = false)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        if (dispose)
            builder.Services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>(_ => new SerilogLoggerProvider(logger, true));
        else
            builder.AddProvider(new SerilogLoggerProvider(logger));
        // builder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
        return builder;
    }
}
