using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Domain.Services;

namespace TraderForge.Application.Handlers;

public class GetMarketPricesQueryHandler
{
    private readonly IMarketService _marketService;
    
    public GetMarketPricesQueryHandler(IMarketService marketService) => _marketService = marketService;
    
    public async Task<ResultGeneric<Dictionary<string, decimal>>> HandleAsync(GetMarketPricesQuery query)
    {
        var symbols = query.Symbols;
        var allPrices = await _marketService.GetPricesAsync();

        if (allPrices == null) 
            return ResultGeneric<Dictionary<string, decimal>>.Failure("No prices found.");
        
        var requestedPrices = (symbols == null || symbols.Count == 0)
            ? allPrices
            : allPrices.Where(p => symbols.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);

        return ResultGeneric<Dictionary<string, decimal>>.Success(requestedPrices);
    }
}
