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
            return await ExcecuteSubscriptionChange(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> ExcecuteSubscriptionChange(ChangeSubscriptionCommand command)
    {
        var trader = await GetTraderByIdAsync(command.TraderId);
        FreezeActivePortfolio(trader);

        var newSubscriptionPlan = await GetSubscriptionPlanByIdAsync(command.NewPlanId);
        trader.AssignSubscriptionPlan(newSubscriptionPlan);
        
        Portfolio newPortfolio = new Portfolio(command.TraderId, newSubscriptionPlan.InitialVirtualBalance);
        trader.Portfolios.Add(newPortfolio);
            
        await _traderRepository.SaveChangesAsync();
            
        return Result.Success();
    }

    private async Task<Trader> GetTraderByIdAsync(string traderId)
    {
        var trader = await _traderRepository.GetByIdAsync(traderId);
        if (trader == null) 
        {
            throw new Exception("Trader not found.");
        }
        return trader;
    }

    private async Task<SubscriptionPlan> GetSubscriptionPlanByIdAsync(Guid id)
    {
        var plan = await _subscriptionPlanRepository.GetByIdAsync(id);
        if (plan == null) 
        {
            throw new Exception("Subscription Plan not found.");
        }
        return plan;
    }

    private void FreezeActivePortfolio(Trader trader)
    {
        var activePortfolio = trader.Portfolios.FirstOrDefault(p => p.IsActive);
    
        if (activePortfolio != null)
        {
            activePortfolio.FreezeSimulation();
        }
    }


}