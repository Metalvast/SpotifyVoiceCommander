using Microsoft.Maui.LifecycleEvents;

namespace SpotifyVoiceCommander.Maui;

partial class MauiProgram
{
    private static partial void AddPlatformServices(this IServiceCollection services)
    { }

    private static partial void ConfigurePlatformLifecycleEvents(ILifecycleBuilder events) =>
        events.AddWindows(windowsLifecycleBuilder =>
        {
            windowsLifecycleBuilder.OnWindowCreated(window =>
            {
                window.ExtendsContentIntoTitleBar = false;
                var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                switch (appWindow.Presenter)
                {
                    case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                        //disable the max button
                        overlappedPresenter.IsMaximizable = false;
                        break;
                }

                //When user execute the closing method, we can make the window do not close by   e.Cancel = true;.
                //appWindow.Closing += (s, e) =>
                //{
                //    e.Cancel = true;
                //};
            });
        });
}
