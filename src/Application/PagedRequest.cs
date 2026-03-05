namespace Application;

public sealed record PagedRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}