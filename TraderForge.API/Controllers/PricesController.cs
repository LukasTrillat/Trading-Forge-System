using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.Handlers;
namespace TraderForge.API.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly GetMarketPricesHandler _getMarketPricesHandler;
    public PricesController(GetMarketPricesHandler getMarketPricesHandler) 
    => _getMarketPricesHandler = getMarketPricesHandler;
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentPrices()
    {
        // TEST //
        var query = new GetMarketPricesQuery(["BTCUSDT", "ETHUSDT", "PAXGUSDT", "SOLUSDT"]);
        //////////
        var result = await _getMarketPricesHandler.GetMarketPricesAsync(query.Symbols);

        if (result.IsSuccess) return Ok(result.Value);
        return NotFound(new { error = result.ErrorMessage });
    }
}