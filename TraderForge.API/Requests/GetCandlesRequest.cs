namespace TraderForge.API.Requests;

public class GetCandlesRequest
{
    public string Symbol { get; set; } = string.Empty;
    public string Interval { get; set; } = "1h";
}
