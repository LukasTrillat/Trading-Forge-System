namespace TraderForge.API.Controllers;
using TraderForge.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly IMarketPriceReader _priceReader;
    
    public PricesController(IMarketPriceReader priceReader) => _priceReader = priceReader;
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentPrices()
    {
        var allPrices = await _priceReader.GetPricesAsync();

        ///// TEMPORAL TEST PRICES /////
        var debugPrices = new List<string> { "BTCUSDT", "ETHUSDT", "PAXGUSDT", "SOLUSDT" };
        ////////////////////////////////
        
        if (allPrices.Count == 0) return NotFound("[Empty pricing information]");
        
        var requestedPrices = allPrices
            .Where(price => debugPrices.Contains(price.Key))
            .ToDictionary(priceSymbol => priceSymbol.Key, priceValue => priceValue.Value);

        return Ok(requestedPrices);
    }
}