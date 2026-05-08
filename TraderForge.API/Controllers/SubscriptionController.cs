using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraderForge.API.Mappers;
using TraderForge.API.Requests;
using TraderForge.Application.DTOs;
using TraderForge.Application.Handlers;

namespace TraderForge.API.Controllers;
[ApiController]
[Route("/api/[controller]")]
[Authorize(Roles = "Trader")]
public class SubscriptionController : ControllerBase
{
    private readonly ChangeSubscriptionCommandHandler _commandHandler;

    public SubscriptionController(ChangeSubscriptionCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    [HttpPost("change-plan")]
    public async Task<IActionResult> ChangePlan([FromBody] ChangeSubscriptionRequest request)
    {
        var traderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(traderId)) return Unauthorized(new { error = "Invalid token claims." });

        var command = request.ToCommand(traderId);

        var result = await _commandHandler.ChangeTraderSubscription(command);

        if (result.IsSuccess) return Ok(new { message = "Subscription succesfully updated! " });
        return BadRequest(new { error = result.ErrorMessage });

    }
}