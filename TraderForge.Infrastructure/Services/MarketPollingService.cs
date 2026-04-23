using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Interfaces;
namespace TraderForge.Infrastructure.Services;

public class MarketPollingService : BackgroundService
{
    private readonly IMarketPriceFetcher _priceFetcher;
    private readonly IMemoryCache _cache; 

    public MarketPollingService(IMarketPriceFetcher priceFetcher, IMemoryCache cache)
    {
        _priceFetcher = priceFetcher;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try 
            {
                var allPrices = await _priceFetcher.GetPricesAsync();
                _cache.Set(CacheKeys.MarketPrices, allPrices, TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Polling Error]: {ex.Message}");
            }
        }
    }
}