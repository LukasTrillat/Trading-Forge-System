using Microsoft.AspNetCore.Mvc;
using TraderForge.API.Mappers;
using TraderForge.API.Requests;
using TraderForge.Application.Handlers;

namespace TraderForge.API.Controllers;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly GetMarketPricesQueryHandler _pricesHandler;
    private readonly GetCandlesQueryHandler _candlesHandler;

    public PricesController(
        GetMarketPricesQueryHandler pricesHandler,
        GetCandlesQueryHandler candlesHandler)
    {
        _pricesHandler = pricesHandler;
        _candlesHandler = candlesHandler;
    }

    [HttpPost]
    public async Task<IActionResult> GetPrices([FromBody] GetMarketPricesRequest request) 
    {
        if (request.Symbols == null || request.Symbols.Count == 0)
        {
            return BadRequest("You must provide at least one symbol.");
        }

        var query = request.ToQuery();
        var result = await _pricesHandler.HandleAsync(query); 
        return Ok(result.Value); 
    }

    [HttpPost("candles")]
    public async Task<IActionResult> GetCandles([FromBody] GetCandlesRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Symbol))
            return BadRequest("Symbol is required.");

        var query = request.ToQuery();
        var result = await _candlesHandler.HandleAsync(query);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Value);
    }
}