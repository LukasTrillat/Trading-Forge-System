using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.Handlers;
namespace TraderForge.API.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly GetMarketPricesQueryHandler _getMarketPricesQueryHandler;
    public PricesController(GetMarketPricesQueryHandler getMarketPricesQueryHandler) 
    => _getMarketPricesQueryHandler = getMarketPricesQueryHandler;
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentPrices()
    {
        // TEST //
        var query = new GetMarketPricesQuery(["BTCUSDT", "ETHUSDT", "PAXGUSDT", "SOLUSDT"]);
        //////////
        var result = await _getMarketPricesQueryHandler.GetMarketPricesAsync(query.Symbols);

        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(new { error = result.ErrorMessage });
    }
}