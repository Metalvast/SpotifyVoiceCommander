namespace SpotifyVoiceCommander.Maui.Shared.Lib.NavigationManager;

public class SvcRoutes
{
    public class Identity
    {
        public const string BasePath = "identity";

        public const string SignIn = $"{BasePath}/sign-in";
    }

    public class AudioPlayer
    {
        public const string BasePath = "audio-player";

        public const string Main = $"{BasePath}/main";
    }
}
