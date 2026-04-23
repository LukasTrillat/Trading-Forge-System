namespace TraderForge.Domain.Interfaces;

public interface IMarketDataProvider
{
    Task<Dictionary<string, decimal>> GetPricesAsync();
}