namespace TraderForge.Domain.Interfaces;

public interface IMarketPriceFetcher
{
    Task<Dictionary<string, decimal>> GetPricesAsync();
}