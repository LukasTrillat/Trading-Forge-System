using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Services;
using TraderForge.Infrastructure.Persistence;

namespace TraderForge.Infrastructure.Services;

public class BackgroundMarketPollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMarketDataProvider _dataProvider;
    private readonly IMemoryCache _cache;
    private readonly IMarketDataBroadcaster _broadcaster;
    
    public BackgroundMarketPollingService(
        IServiceScopeFactory scopeFactory, 
        IMarketDataProvider dataProvider, 
        IMemoryCache cache,
        IMarketDataBroadcaster broadcaster)
    {
        _scopeFactory = scopeFactory;
        _dataProvider = dataProvider;
        _cache = cache;
        _broadcaster = broadcaster;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ExecutePollingCycle(stoppingToken);
        }
    }
    
    private async Task ExecutePollingCycle(CancellationToken stoppingToken)
    {
        try
        {
            var allPrices = await _dataProvider.GetPricesAsync();
            SaveToCache(allPrices);
            await SaveToDatabase(allPrices);
            await _broadcaster.BroadCastPricesAsync(allPrices, stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Polling Error]: {ex.Message}");
        }
    }
    
    private void SaveToCache(Dictionary<string, decimal> allPrices)
    {
        _cache.Set(CacheKeys.MarketPrices, allPrices, TimeSpan.FromSeconds(30));
    }

    private async Task SaveToDatabase(Dictionary<string, decimal> allPrices)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var now = DateTime.UtcNow;

        foreach (var symbol in SupportedAssets.Symbols)
        {
            if (!allPrices.TryGetValue(symbol, out var price))
                continue;

            var existing = await db.MarketAssets.FirstOrDefaultAsync(a => a.Symbol == symbol);
            if (existing != null)
            {
                existing.CurrentPrice = price;
                existing.LastUpdated = now;
            }
            else
            {
                db.MarketAssets.Add(new MarketAsset
                {
                    Symbol = symbol,
                    Name = symbol,
                    CurrentPrice = price,
                    LastUpdated = now,
                });
            }

            db.PriceSnapshots.Add(new PriceSnapshot(symbol, price, now));
        }

        await db.SaveChangesAsync();
    }
}