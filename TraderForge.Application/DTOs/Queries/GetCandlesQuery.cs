namespace TraderForge.Application.DTOs.Queries;

public class GetCandlesQuery
{
    public required string Symbol { get; set; }
    public required string Interval { get; set; }
}
