using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class ChangeSubscriptionCommandHandler
{
    private readonly ITraderRepository _traderRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public ChangeSubscriptionCommandHandler(ITraderRepository traderRepository, ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _traderRepository = traderRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<Result> ChangeTraderSubscription(ChangeSubscriptionCommand command)
    {
        try
        {
            return await ExecuteSubscriptionChange(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> ExecuteSubscriptionChange(ChangeSubscriptionCommand command) 
    {
        var trader = await _traderRepository.GetByIdAsync(command.TraderId);
        if (trader == null) 
        {
            return Result.Failure("Trader not found.");
        }

        var newSubscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(command.NewPlanId);
        if (newSubscriptionPlan == null) 
        {
            return Result.Failure("Subscription Plan not found.");
        }

        trader.ChangeSubscriptionPlan(newSubscriptionPlan);
        
        await _traderRepository.SaveChangesAsync();
        
        return Result.Success();
    }


}