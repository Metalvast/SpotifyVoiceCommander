using Serilog.Core;
using Serilog.Events;
using System.Globalization;

namespace SpotifyVoiceCommander.Maui.Shared.Helpers;

internal class ThreadIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var managedThreadId = Environment.CurrentManagedThreadId.ToString("D4", CultureInfo.InvariantCulture);
        var threadId = managedThreadId;
#if ANDROID
        var myTid = Android.OS.Process.MyTid();
        threadId = threadId + "-" + myTid;
#endif
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadID", threadId));
    }
}
