using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class GetMarketPricesQueryHandler
{
    private readonly IMarketService _marketService;
    
    public GetMarketPricesQueryHandler(IMarketService marketService) => _marketService = marketService;
    
    public async Task<Result<Dictionary<string, decimal>>> GetMarketPricesAsync(GetMarketPricesQuery query)
    {
        var symbols = query.Symbols;
        
        var allPrices = await _marketService.GetPricesAsync();
        if (allPrices.Count == 0) return Result<Dictionary<string, decimal>>.Failure("No prices found.");
        
        var requestedPrices = allPrices
            .Where(priceSymbol => symbols.Contains(priceSymbol.Key))
            .ToDictionary(priceValue => priceValue.Key, p => p.Value);

        return Result<Dictionary<string, decimal>>.Success(requestedPrices);
    }
}