namespace WebApplication1.Models;

public record YandexCloudApiSettings
{
    public required string ApiKey { get; init; }
    public required string FolderId { get; init; }
    public required string Prompt { get; init; }
}
