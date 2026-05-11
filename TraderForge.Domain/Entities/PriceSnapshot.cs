namespace TraderForge.Domain.Entities;

public class PriceSnapshot
{
    public long Id { get; private set; }
    public string Symbol { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateTime RecordedAt { get; private set; }

    private PriceSnapshot() { }

    public PriceSnapshot(string symbol, decimal price, DateTime recordedAt)
    {
        Symbol = symbol;
        Price = price;
        RecordedAt = recordedAt;
    }
}
