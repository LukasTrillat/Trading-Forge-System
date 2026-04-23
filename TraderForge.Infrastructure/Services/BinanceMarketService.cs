using System.Net.Http.Json;
using System.Globalization;
using TraderForge.Domain.Interfaces;
namespace TraderForge.Infrastructure.Services;

<<<<<<<< HEAD:TraderForge.Infrastructure/Services/BinanceMarketProvider.cs
public class BinanceMarketProvider : IMarketDataProvider
========
public class BinanceMarketService : IMarketDataProvider
>>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba:TraderForge.Infrastructure/Services/BinanceMarketService.cs
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "https://api.binance.com/api/v3/ticker/price";
    
<<<<<<<< HEAD:TraderForge.Infrastructure/Services/BinanceMarketProvider.cs
    public BinanceMarketProvider(HttpClient client) => _client = client;
========
    public BinanceMarketService(HttpClient client) => _client = client;
>>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba:TraderForge.Infrastructure/Services/BinanceMarketService.cs

    public async Task<Dictionary<string, decimal>> GetPricesAsync()
    {
        var allPrices = await _client.GetFromJsonAsync<List<BinancePrice>>(_baseUrl);
        if (allPrices == null) return new Dictionary<string, decimal>();
        
        return allPrices.ToDictionary(
            priceSymbol => priceSymbol.symbol, 
            priceValue => decimal.Parse(priceValue.price, CultureInfo.InvariantCulture)
        );
    }
}

public record BinancePrice(string symbol, string price);