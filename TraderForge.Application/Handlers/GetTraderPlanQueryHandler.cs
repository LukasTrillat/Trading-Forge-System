using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;
using TraderForge.Domain.Repositories;

namespace TraderForge.Application.Handlers;

public class GetTraderPlanQueryHandler
{
    private readonly ITraderRepository _traderRepository;
    public GetTraderPlanQueryHandler(ITraderRepository traderRepository)
    {
        _traderRepository = traderRepository;
    }
    public async Task<Result<SubscriptionPlan?>> HandleAsync(GetTraderPlanQuery query)
    {
        try
        {
            var trader = await _traderRepository.GetByIdWithSubscriptionPlanAsync(query.TraderId);
            
            if (trader is null)
                return Result<SubscriptionPlan?>.Failure("Trader not found.");
            return Result<SubscriptionPlan?>.Success(trader.SubscriptionPlan);
            
        }
        catch (Exception ex)
        {
            return Result<SubscriptionPlan?>.Failure(ex.Message);
        }
    }
}