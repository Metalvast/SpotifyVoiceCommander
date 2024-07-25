namespace SpotifyVoiceCommander.Maui.Entities.SpeechRecognizer.Models;

internal class SpeechRecognizerErrors
{
    public static readonly Error RecognizeFailed = Error.Custom(1000, "SpeechRecognizer.RecognizeFailed", "Recognize failed");

    public static readonly Error AnalyzeFailed = Error.Custom(1100, "SpeechRecognizer.AnalyzeFailed", "Analyze failed");
}
