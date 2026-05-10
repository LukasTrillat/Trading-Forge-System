using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Services;

namespace TraderForge.Application.Handlers;

public class SellPositionCommandHandler
{
    private readonly IPositionRepository _positionRepository;
    private readonly ICommissionService _commissionService;

    public SellPositionCommandHandler(
        IPositionRepository positionRepository,
        ICommissionService commissionService)
    {
        _positionRepository = positionRepository;
        _commissionService = commissionService;
    }

    public async Task<Result> HandleAsync(SellPositionCommand command)
    {
        try
        {
            return await SellAsync(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> SellAsync(SellPositionCommand command)
    {
        var position = await _positionRepository.GetByIdWithPortfolioAsync(command.PositionId);
        if (position == null)
            return Result.Failure("Position not found.");

        var portfolio = position.Portfolio;
        var proceeds = CalculateProceeds(position);
        var commission = _commissionService.Calculate(proceeds);
        var netProceeds = proceeds - commission;

        portfolio.AddFunds(netProceeds, "Sell", position.Symbol, position.Quantity, position.EntryPrice, commission);
        await _positionRepository.RemoveAsync(position);
        return Result.Success();
    }

    private static decimal CalculateProceeds(Position position)
    {
        return position.Quantity * position.EntryPrice;
    }
}
