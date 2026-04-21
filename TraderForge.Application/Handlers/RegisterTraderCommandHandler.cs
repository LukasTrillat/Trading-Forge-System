using System.Runtime.InteropServices.JavaScript;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class RegisterTraderCommandHandler
{
    private readonly IIdentityService _identityService;
    private readonly ITraderRepository _traderRepository;

    public RegisterTraderCommandHandler(IIdentityService identityService, ITraderRepository traderRepository)
    {
        _identityService = identityService;
        _traderRepository = traderRepository;
    }

    public async Task Handle(RegisterTraderCommand command)
    {
        string newId = await _identityService.RegisterUserAndReturnIdAsync(command.Email, command.Password);
        Trader newTrader = new Trader(newId, command.Email);
        newTrader.FreeTrialRegistrationDate = DateTime.UtcNow;
        newTrader.FreeTrialExpirationDate = DateTime.UtcNow.AddDays(7);
        newTrader.UserName = command.Email;
        await _traderRepository.AddAsync(newTrader);
    }
    
}