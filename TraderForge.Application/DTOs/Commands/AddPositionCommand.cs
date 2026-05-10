namespace TraderForge.Application.DTOs;

public class AddPositionCommand
{
    public string TraderId { get; set; }
    public string Symbol { get; set; }
    public decimal Quantity { get; set; }
    public decimal EntryPrice { get; set; }
}
