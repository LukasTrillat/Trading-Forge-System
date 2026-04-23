<<<<<<< HEAD
﻿using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Domain.Interfaces;
=======
﻿using TraderForge.Domain.Interfaces;
using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba

namespace TraderForge.Application.Handlers;

public class GetMarketPricesQueryHandler
{
    private readonly IMarketService _marketService;
    
    public GetMarketPricesQueryHandler(IMarketService marketService) => _marketService = marketService;
    
<<<<<<< HEAD
    public async Task<ResultGeneric<Dictionary<string, decimal>>> GetMarketPricesAsync(GetMarketPricesQuery query)
=======
    public async Task<Result<Dictionary<string, decimal>>> GetMarketPricesAsync(GetMarketPricesQuery query)
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
    {
        var symbols = query.Symbols;
        
        var allPrices = await _marketService.GetPricesAsync();
<<<<<<< HEAD
        if (allPrices.Count == 0) return ResultGeneric<Dictionary<string, decimal>>.Failure("No prices found.");
=======
        if (allPrices.Count == 0) return Result<Dictionary<string, decimal>>.Failure("No prices found.");
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
        
        var requestedPrices = allPrices
            .Where(priceSymbol => symbols.Contains(priceSymbol.Key))
            .ToDictionary(priceValue => priceValue.Key, p => p.Value);

<<<<<<< HEAD
        return ResultGeneric<Dictionary<string, decimal>>.Success(requestedPrices);
=======
        return Result<Dictionary<string, decimal>>.Success(requestedPrices);
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
    }
}