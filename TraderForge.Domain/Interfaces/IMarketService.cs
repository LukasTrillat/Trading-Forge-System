namespace TraderForge.Domain.Interfaces;

public interface IMarketService
{
    Task<Dictionary<string, decimal>> GetPricesAsync();
}