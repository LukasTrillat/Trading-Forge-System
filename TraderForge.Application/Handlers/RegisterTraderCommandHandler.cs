using System.Runtime.InteropServices.JavaScript;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class RegisterTraderCommandHandler
{
    public IIdentityService IdentityService;
    public ITraderRepository TraderRepository;

    RegisterTraderCommandHandler(IIdentityService identityService, ITraderRepository traderRepository)
    {
        IdentityService = identityService;
        TraderRepository = traderRepository;
    }

    public async Task Handle(RegisterTraderCommand command)
    {
        string newId = await IdentityService.RegisterUserAndReturnIdAsync(command.Email, command.Password);
        Trader newTrader = new Trader(command.Email, newId);
        newTrader.FreeTrialRegistrationDate = DateTime.UtcNow;
        newTrader.FreeTrialExpirationDate = DateTime.UtcNow.AddDays(7);
        await TraderRepository.AddAsync(newTrader);
    }
    
}