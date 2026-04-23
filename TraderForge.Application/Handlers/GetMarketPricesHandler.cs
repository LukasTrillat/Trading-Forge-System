using TraderForge.Domain.Interfaces;
using TraderForge.Application.Common;
namespace TraderForge.Application.Handlers;

public class GetMarketPricesHandler
{
    private readonly IMarketPriceReader _priceReader;
    
    public GetMarketPricesHandler(IMarketPriceReader priceReader) => _priceReader = priceReader;
    
    public async Task<Result<Dictionary<string, decimal>>> GetMarketPricesAsync(List<string> symbols)
    {
        var allPrices = await _priceReader.GetPricesAsync();
        if (allPrices.Count == 0) return new Result<Dictionary<string, decimal>>();
        
        var requestedPrices = allPrices
            .Where(priceSymbol => symbols.Contains(priceSymbol.Key))
            .ToDictionary(priceValue => priceValue.Key, p => p.Value);

        return Result<Dictionary<string, decimal>>.Success(requestedPrices);
    }
}