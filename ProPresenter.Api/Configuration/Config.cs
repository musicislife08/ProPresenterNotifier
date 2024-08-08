namespace ProPresenter.Api.Configuration;

public record Config
{
    public string? BaseUrl { get; init; }
    public string? MessageName { get; init; }
    public string? AppName { get; init; }
}