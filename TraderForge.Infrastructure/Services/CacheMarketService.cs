using Microsoft.Extensions.Caching.Memory;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Interfaces;

public class CacheMarketService : IMarketService
{
    private readonly IMemoryCache _cache;
    
    public CacheMarketService(IMemoryCache cache) => _cache = cache;
    
    public async Task<Dictionary<string, decimal>> GetPricesAsync()
    {
        if (_cache.TryGetValue(CacheKeys.MarketPrices, out Dictionary<string, decimal> cachedPrices))
        {
            return cachedPrices;
        }

        return new Dictionary<string, decimal>();
    }
}