using System.Text.Json.Serialization;

namespace ProPresenter.Api.Messages;

public record TextToken
{
    [JsonPropertyName("text")]
    public string? TextContent { get; init; }
}

