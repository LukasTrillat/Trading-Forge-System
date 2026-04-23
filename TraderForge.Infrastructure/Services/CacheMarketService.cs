using Microsoft.Extensions.Caching.Memory;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Interfaces;

<<<<<<<< HEAD:TraderForge.Infrastructure/Services/CachedMarketService.cs
public class CachedMarketService : IMarketService
{
    private readonly IMemoryCache _cache;
    
    public CachedMarketService(IMemoryCache cache) => _cache = cache;
========
public class CacheMarketService : IMarketService
{
    private readonly IMemoryCache _cache;
    
    public CacheMarketService(IMemoryCache cache) => _cache = cache;
>>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba:TraderForge.Infrastructure/Services/CacheMarketService.cs
    
    public async Task<Dictionary<string, decimal>> GetPricesAsync()
    {
        if (_cache.TryGetValue(CacheKeys.MarketPrices, out Dictionary<string, decimal> cachedPrices))
        {
            return cachedPrices;
        }

        return new Dictionary<string, decimal>();
    }
}