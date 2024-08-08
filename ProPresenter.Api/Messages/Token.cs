namespace ProPresenter.Api.Messages;

public record Token
{
    public string? Name { get; init; }
    public TextToken? Text { get; init; }
};