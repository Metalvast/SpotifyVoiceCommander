namespace SpotifyVoiceCommander.Maui.Shared.UI;

public readonly struct TagId
{
    public string Tag { get; }
    public Guid Value { get; }

    public TagId(string tag)
    {
        Tag = tag.ToKebabCase();
        Value = Guid.NewGuid();
    }

    public override string ToString() => this;

    public static implicit operator string(TagId id) =>
        $"{id.Tag}:{id.Value}";

    public static implicit operator Guid(TagId id) =>
        id.Value;
}
