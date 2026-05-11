using Microsoft.AspNetCore.Mvc;
using TraderForge.API.Mappers;
using TraderForge.API.Requests;
using TraderForge.Application.Handlers;
using TraderForge.Application.DTOs.Queries;

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

    [HttpGet]
    public async Task<IActionResult> GetAllPrices()
    {
        var result = await _handler.HandleAsync(new GetMarketPricesQuery { Symbols = new List<string>() });
        
        if (!result.IsSuccess) return Ok(new List<object>());

        var assets = result.Value.Select(kvp => new 
        { 
            symbol = kvp.Key, 
            name = kvp.Key,
            currentPrice = kvp.Value,
            priceChange24h = 0,
            priceChangePercent24h = 0 
        }).ToList();

        return Ok(assets);
    }

    [HttpPost]
    public async Task<IActionResult> GetPrices([FromBody] GetMarketPricesRequest request) 
    {
        var result = await _handler.HandleAsync(request.ToQuery());
        
        if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });

        var assets = result.Value.Select(kvp => new 
        { 
            symbol = kvp.Key, 
            currentPrice = kvp.Value 
        }).ToList();

        return Ok(assets); 
    }
}
