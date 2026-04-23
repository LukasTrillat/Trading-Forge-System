using TraderForge.Domain.Interfaces;
using TraderForge.Application.Common;
namespace TraderForge.Application.Handlers;

public class GetMarketPricesQueryHandler
{
    private readonly IMarketService _marketService;
    
    public GetMarketPricesQueryHandler(IMarketService marketService) => _marketService = marketService;
    
    public async Task<Result<Dictionary<string, decimal>>> GetMarketPricesAsync(List<string> symbols)
    {
        var allPrices = await _marketService.GetPricesAsync();
        if (allPrices.Count == 0) return new Result<Dictionary<string, decimal>>();
        
        var requestedPrices = allPrices
            .Where(priceSymbol => symbols.Contains(priceSymbol.Key))
            .ToDictionary(priceValue => priceValue.Key, p => p.Value);

        return Result<Dictionary<string, decimal>>.Success(requestedPrices);
    }
}