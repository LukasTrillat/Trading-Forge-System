using Microsoft.AspNetCore.Mvc;
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

        var assets = result.Value.Select(asset => new { 
            symbol = asset.Symbol, 
            name = asset.Name,
            currentPrice = asset.CurrentPrice,
            priceChange24h = 0.5m, 
            priceChangePercent24h = 1.2m 
        }).ToList();

        return Ok(assets);
    }

    [HttpGet("{symbol}/candles")]
    public IActionResult GetCandles(string symbol)
    {
        // SIMULATOR: Generates 50 fake candles so the chart works immediately
        var random = new Random();
        var candles = new List<object>();
        decimal lastPrice = 50000;

        for (int i = 0; i < 50; i++)
        {
            decimal open = lastPrice;
            decimal close = open + (decimal)(random.NextDouble() * 100 - 50);
            decimal high = Math.Max(open, close) + (decimal)(random.NextDouble() * 20);
            decimal low = Math.Min(open, close) - (decimal)(random.NextDouble() * 20);
            
            candles.Add(new { 
                t = DateTimeOffset.UtcNow.AddMinutes(-i * 15).ToUnixTimeMilliseconds(),
                open, high, low, close,
                volume = random.Next(100, 1000)
            });
            lastPrice = close;
        }

        return Ok(candles);
    }

    [HttpGet("{symbol}/orderbook")]
    public IActionResult GetOrderBook(string symbol)
    {
        return Ok(new { 
            bids = new List<object> { new { price = 49990, quantity = 1.5 } },
            asks = new List<object> { new { price = 50010, quantity = 0.8 } }
        });
    }
}
