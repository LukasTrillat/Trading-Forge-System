using Microsoft.AspNetCore.Mvc;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.Handlers;
namespace TraderForge.API.Controllers;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly GetMarketPricesQueryHandler _handler;

    public PricesController(GetMarketPricesQueryHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> GetPrices([FromBody] GetMarketPricesQuery query) 
    {
        if (query.Symbols == null || query.Symbols.Count == 0)
        {
            return BadRequest("You must provide at least one symbol.");
        }

        var result = await _handler.GetMarketPricesAsync(query); 
        
        return Ok(result.Value); 
    }
}