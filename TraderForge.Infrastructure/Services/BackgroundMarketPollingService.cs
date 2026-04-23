using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Interfaces;
namespace TraderForge.Infrastructure.Services;

public class BackgroundMarketPollingService : BackgroundService
{
    private readonly IMarketDataProvider _dataProvider;
    private readonly IMemoryCache _cache; 

    public BackgroundMarketPollingService(IMarketDataProvider dataProvider, IMemoryCache cache)
    {
        _dataProvider = dataProvider;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(0.1));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try 
            {
                var allPrices = await _dataProvider.GetPricesAsync();
                _cache.Set(CacheKeys.MarketPrices, allPrices, TimeSpan.FromMinutes(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Polling Error]: {ex.Message}");
            }
        }
    }
}