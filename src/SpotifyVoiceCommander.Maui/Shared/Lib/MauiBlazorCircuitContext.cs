using ActualLab.Fusion.Blazor;
using Microsoft.Extensions.Logging;

namespace SpotifyVoiceCommander.Maui.Shared.Lib;

public sealed class MauiBlazorCircuitContext : BlazorCircuitContext
{
    public MauiBlazorCircuitContext(IServiceProvider services) : base(services)
    {
        Log.LogInformation("[+] #{Id}", Id);
    }

    protected override Task DisposeAsyncCore()
    {
        Log.LogInformation("[-] #{Id}", Id);

        return Task.CompletedTask;
    }
}
