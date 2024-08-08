using System.Text.Json.Serialization;

namespace ProPresenter.Api.Messages;

public record Messages
{
    public Id? Id { get; init; }
    public string? Message { get; init; }
    public Token[] Tokens { get; init; } = [];
    public Theme? Theme { get; init; }
    [JsonPropertyName("visible_on_network")]
    public bool VisibleOnNetwork { get; init; }
}