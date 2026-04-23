<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.Handlers;
=======
﻿// TraderForge.API/Controllers/PricesController.cs
using Microsoft.AspNetCore.Mvc;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.Handlers;

>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
namespace TraderForge.API.Controllers;

[ApiController]
[Route("api/prices")]
public class PricesController : ControllerBase
{
    private readonly GetMarketPricesQueryHandler _handler;

    public PricesController(GetMarketPricesQueryHandler handler)
    {
        _handler = handler;
<<<<<<< HEAD
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
=======
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
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
