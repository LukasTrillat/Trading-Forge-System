using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;
using TraderForge.Domain.Constants;
using TraderForge.Domain.Services;

namespace TraderForge.Infrastructure.Services;

public class BinanceMarketProvider : IMarketDataProvider
{
    private readonly HttpClient _client;
    private static readonly string _requestUrl = BuildRequestUrl();
    
    public BinanceMarketProvider(HttpClient client) => _client = client;

    public async Task<Dictionary<string, decimal>> GetPricesAsync()
    {
        var allPrices = await _client.GetFromJsonAsync<List<BinancePrice>>(_requestUrl);
        if (allPrices == null) return new Dictionary<string, decimal>();
        
        return allPrices.ToDictionary(
            priceSymbol => priceSymbol.symbol, 
            priceValue => decimal.Parse(priceValue.price, CultureInfo.InvariantCulture)
        );
    }

    private static string BuildRequestUrl()
    {
        var symbolsJson = JsonSerializer.Serialize(SupportedAssets.Symbols);
        return $"https://api.binance.com/api/v3/ticker/price?symbols={Uri.EscapeDataString(symbolsJson)}";
    }
}

public record BinancePrice(string symbol, string price);