using Microsoft.AspNetCore.Mvc;
using TraderForge.Application.DTOs;
using TraderForge.Application.Handlers;

namespace TraderForge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly RegisterTraderCommandHandler _registerTraderCommandHandler;

    public IdentityController(RegisterTraderCommandHandler registerTraderCommandHandler)
    {
        _registerTraderCommandHandler = registerTraderCommandHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterTraderCommand command)
    {
        try
        {
            await _registerTraderCommandHandler.Handle(command);
            return Ok(new { message = "Registration succesful! Enjoy your 7-Day free trial." });

        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        
    }
}