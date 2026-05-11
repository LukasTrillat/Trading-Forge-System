using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Services;

namespace TraderForge.Infrastructure.Services;

public class BackgroundMarketPollingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    // Define only the symbols you want to support
    private readonly string[] _supportedSymbols = { "BTCUSDT", "ETHUSDT", "SOLUSDT", "BNBUSDT", "ADAUSDT", "DOTUSDT", "MATICUSDT" };

    public BackgroundMarketPollingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[BackgroundService] Market Polling Service Started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dataProvider = scope.ServiceProvider.GetRequiredService<IMarketDataProvider>();
                var repository = scope.ServiceProvider.GetRequiredService<IMarketAssetRepository>();

                var allPrices = await dataProvider.GetPricesAsync();
                
                if (allPrices != null && allPrices.Any())
                {
                    // FILTER: Only keep prices for our supported symbols
                    var filteredPrices = allPrices
                        .Where(p => _supportedSymbols.Contains(p.Key))
                        .ToDictionary(p => p.Key, p => p.Value);

                    var existingAssets = (await repository.GetAllAsync()).ToList();

                    foreach (var price in filteredPrices)
                    {
                        var existingAsset = existingAssets.FirstOrDefault(a => a.Symbol == price.Key);

                        if (existingAsset != null)
                        {
                            existingAsset.CurrentPrice = price.Value;
                            existingAsset.LastUpdated = DateTime.UtcNow;
                            await repository.UpdateAsync(existingAsset);
                        }
                        else
                        {
                            Console.WriteLine($"[BackgroundService] Seeding: {price.Key}");
                            await repository.AddAsync(new MarketAsset
                            {
                                Symbol = price.Key,
                                Name = price.Key.Replace("USDT", ""), // Clean name (e.g. BTC)
                                CurrentPrice = price.Value,
                                LastUpdated = DateTime.UtcNow
                            });
                        }
                    }

                    await repository.SaveChangesAsync();
                    Console.WriteLine($"[BackgroundService] Successfully synced {filteredPrices.Count} assets.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BackgroundService] ERROR: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
