﻿using Android.Webkit;
using AndroidX.Activity;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;
using AndroidWebView = Android.Webkit.WebView;
using MixedContentHandling = Android.Webkit.MixedContentHandling;

namespace SpotifyVoiceCommander.Maui.Shared.Lib.WebView;

public partial class MauiWebView
{
    public AndroidWebView AndroidWebView { get; private set; } = null!;

    public partial void SetPlatformWebView(object platformWebView)
    {
        if (ReferenceEquals(PlatformWebView, platformWebView))
            return;

        PlatformWebView = platformWebView;
        AndroidWebView = (AndroidWebView)platformWebView;
    }

    public partial void HardNavigateTo(string url)
        => AndroidWebView.LoadUrl(url);

    public partial Task EvaluateJavaScript(string javaScript)
    {
        var request = new EvaluateJavaScriptAsyncRequest(javaScript);
        AndroidWebView.EvaluateJavaScript(request);
        return request.Task;
    }

    // Private methods

    private partial void OnInitializing(object? sender, BlazorWebViewInitializingEventArgs eventArgs)
    { }

    private partial void OnInitialized(object? sender, BlazorWebViewInitializedEventArgs eventArgs)
    {
        var webView = eventArgs.WebView;
        SetPlatformWebView(webView);
        if (webView.Context?.GetActivity() is not ComponentActivity activity)
            throw new InvalidOperationException(
                $"The permission-managing WebChromeClient requires that the current activity is a '{nameof(ComponentActivity)}'.");

        var settings = webView.Settings;
        settings.JavaScriptEnabled = true;
        settings.AllowFileAccess = true;
        settings.MediaPlaybackRequiresUserGesture = false;
        settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
        settings.CacheMode = CacheModes.Default;
        // settings.OffscreenPreRaster = true;
#pragma warning disable CS0618
        settings.EnableSmoothTransition();
#pragma warning restore CS0618
        webView.SetRendererPriorityPolicy(RendererPriority.Important, true);

        // AndroidJSInterface methods will be available for invocation in js via 'window.Android' object.
        //var jsInterface = new AndroidJSInterface(webView);
        //webView.AddJavascriptInterface(jsInterface, "Android");

        //webView.SetWebViewClient(
        //    new AndroidWebViewClient(
        //        webView.WebViewClient,
        //        AppServices.GetRequiredService<AndroidContentDownloader>(),
        //        AppServices.LogFor<AndroidWebViewClient>()));

        //webView.SetWebChromeClient(
        //    new AndroidWebChromeClient(
        //        webView.WebChromeClient!,
        //        activity,
        //        new AndroidFileChooser(AppServices.LogFor<AndroidFileChooser>())));
    }

    private partial void OnLoaded(object? sender, EventArgs eventArgs) { }

    //private partial void SetupSessionCookie(Session session)
    //{
    //    var webView = AndroidWebView;
    //    if (webView.IsNull())
    //        return;

    //    var cookieManager = CookieManager.Instance!;
    //    var cookieName = Constants.Session.CookieName;
    //    var sessionId = session.Id.Value;

    //    // May be will be required https://stackoverflow.com/questions/2566485/webview-and-cookies-on-android
    //    cookieManager.SetAcceptCookie(true);
    //    cookieManager.SetAcceptThirdPartyCookies(AndroidWebView, true);
    //    var sessionCookieValue = $"{cookieName}={sessionId}; path=/; secure; samesite=none; httponly";
    //    cookieManager.SetCookie("https://" + MauiSettings.LocalHost, sessionCookieValue);
    //    cookieManager.SetCookie("https://" + MauiSettings.Host, sessionCookieValue);
    //}
}
