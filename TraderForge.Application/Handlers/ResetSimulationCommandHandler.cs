using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;

namespace TraderForge.Application.Handlers;

public class ResetSimulationCommandHandler
{
    private readonly ITraderRepository _traderRepository;

    public ResetSimulationCommandHandler(ITraderRepository traderRepository)
    {
        _traderRepository = traderRepository;
    }

    public async Task<Result> HandleAsync(ResetSimulationCommand command)
    {
        try
        {
            return await ExecuteAsync(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> ExecuteAsync(ResetSimulationCommand command)
    {
        var trader = await _traderRepository.GetByIdIncludePlanAndPositionsAsync(command.TraderId);
        if (trader == null)
            return Result.Failure("Trader not found.");

        var activePortfolio = trader.Portfolios.FirstOrDefault(p => p.IsActive);
        if (activePortfolio == null)
            return Result.Failure("No active portfolio found.");

        var plan = trader.SubscriptionPlan;
        if (plan == null)
            return Result.Failure("No subscription plan assigned.");

        activePortfolio.FreezeSimulation();

        activePortfolio.AddFunds(0, "Reset", null, null, null, 0);

        var newPortfolio = new Portfolio(trader.Id, plan.InitialVirtualBalance);
        trader.Portfolios.Add(newPortfolio);

        newPortfolio.AddFunds(plan.InitialVirtualBalance, "Reset", null, null, null, 0);

        await _traderRepository.SaveChangesAsync();
        return Result.Success();
    }
}
