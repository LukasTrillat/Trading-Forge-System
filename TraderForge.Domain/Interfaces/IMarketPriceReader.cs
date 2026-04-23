namespace TraderForge.Domain.Interfaces;

public interface IMarketPriceReader
{
    Task<Dictionary<string, decimal>> GetPricesAsync();
}