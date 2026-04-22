using Microsoft.AspNetCore.Mvc;
using TraderForge.Application.DTOs;
using TraderForge.Application.Handlers;

namespace TraderForge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly RegisterTraderCommandHandler _registerTraderCommandHandler;
    private readonly LoginTraderQueryHandler _loginTraderQueryHandler;

    public IdentityController(RegisterTraderCommandHandler registerTraderCommandHandler, LoginTraderQueryHandler loginTraderQueryHandler)
    {
        _registerTraderCommandHandler = registerTraderCommandHandler;
        _loginTraderQueryHandler = loginTraderQueryHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterTraderCommand command)
    {

        var result = await _registerTraderCommandHandler.RegisterAsync(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Registration succesful! Enjoy your 7-Day free trial." });
        }

        return BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginTraderQuery query)
    {
        var result = await _loginTraderQueryHandler.LoginAsync(query);

        if (result.IsSuccess)
        {
            return Ok(new { token = result.Value });
        }

        return Unauthorized(new { error = result.ErrorMessage });

    }
}