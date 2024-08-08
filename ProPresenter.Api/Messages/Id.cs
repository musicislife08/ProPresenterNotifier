namespace ProPresenter.Api.Messages;

public record Id
{
    public string? Name { get; init; }
    public int Index { get; init; }
    public string? Uuid { get; init; }
}