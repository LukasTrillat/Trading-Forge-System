using TraderForge.API.Requests;
using TraderForge.Application.DTOs.Queries;

namespace TraderForge.API.Mappers;

public static class PricesMapper
{
    public static GetMarketPricesQuery ToQuery(this GetMarketPricesRequest request)
    {
        return new GetMarketPricesQuery
        {
            Symbols = request.Symbols
        };
    }

    public static GetCandlesQuery ToQuery(this GetCandlesRequest request)
    {
        return new GetCandlesQuery
        {
            Symbol = request.Symbol,
            Interval = request.Interval,
        };
    }
}
