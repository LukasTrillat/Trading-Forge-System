using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Entities;

namespace TraderForge.Application.Handlers;

public class GetMarketPricesQueryHandler
{
    private readonly IMarketAssetRepository _repository;
    
    // We inject the Repository now to read directly from the DB table
    public GetMarketPricesQueryHandler(IMarketAssetRepository repository) 
        => _repository = repository;
    
    public async Task<ResultGeneric<List<MarketAsset>>> HandleAsync(GetMarketPricesQuery query)
    {
        // 1. Get everything from the MarketAssets table
        var allAssets = (await _repository.GetAllAsync()).ToList();

        if (allAssets == null || !allAssets.Any())
        {
            return ResultGeneric<List<MarketAsset>>.Failure("Database table 'MarketAssets' is empty.");
        }

        // 2. Filter if specific symbols were requested, otherwise return all
        var results = (query.Symbols == null || query.Symbols.Count == 0)
            ? allAssets
            : allAssets.Where(a => query.Symbols.Contains(a.Symbol)).ToList();

        return ResultGeneric<List<MarketAsset>>.Success(results);
    }
}
