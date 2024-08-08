namespace ProPresenter.Api.Messages;

public record Theme
{
    public string? Name { get; init; }
    public int Index { get; init; }
    public string? Uuid { get; init; }
}

