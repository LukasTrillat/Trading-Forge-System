using System.Runtime.InteropServices.JavaScript;
using TraderForge.Application.Common;
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

    public async Task<Result> RegisterTraderAsync(RegisterTraderCommand command)
    {
        try
        {
            return await ExecuteRegistration(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> ExecuteRegistration(RegisterTraderCommand command)
    {
        string newUserId = GenerateNewAccountId();
        await _identityService.RegisterNewAccountAsync(newUserId,command.Email, command.Password);

        Trader newTrader = GenerateTraderWithFreeTrial(newUserId, command.Email);
        await _traderRepository.AddAsync(newTrader);
            
        return Result.Success();
    }

    private string GenerateNewAccountId()
    {
        return Guid.NewGuid().ToString();
    }

    private Trader GenerateTraderWithFreeTrial(string id, string email)
    {
        Trader newTrader = new Trader(id, email);
        newTrader.FreeTrialRegistrationDate = DateTime.UtcNow;
        newTrader.FreeTrialExpirationDate = DateTime.UtcNow.AddDays(7);
        newTrader.UserName = email;
        return newTrader;
    }
    
    
    
}