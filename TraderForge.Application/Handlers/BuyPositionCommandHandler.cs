using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Services;

namespace TraderForge.Application.Handlers;

public class BuyPositionCommandHandler
{
    private readonly IPositionRepository _positionRepository;
    private readonly ITraderRepository _traderRepository;
    private readonly ISubscriptionLimitGuard _limitGuard;
    private readonly ICommissionService _commissionService;

    public BuyPositionCommandHandler(
        IPositionRepository positionRepository,
        ITraderRepository traderRepository,
        ISubscriptionLimitGuard limitGuard,
        ICommissionService commissionService)
    {
        _positionRepository = positionRepository;
        _traderRepository = traderRepository;
        _limitGuard = limitGuard;
        _commissionService = commissionService;
    }

    public async Task<Result> HandleAsync(BuyPositionCommand command)
    {
        try
        {
            return await ExecuteTradeAsync(command);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> ExecuteTradeAsync(BuyPositionCommand command)
    {
        var limitResult = await VerifySubscriptionLimitAsync(command.TraderId);
        if (!limitResult.IsSuccess)
            return limitResult;

        var portfolio = await GetActivePortfolioAsync(command.TraderId);
        if (portfolio == null)
            return Result.Failure("No active portfolio found.");

        var totalCost = CalculateTotalCost(command);
        var balanceResult = EnsureSufficientBalance(totalCost, portfolio.VirtualBalance);
        if (!balanceResult.IsSuccess)
            return balanceResult;

        await UpsertPositionAsync(portfolio, command);

        var commission = _commissionService.Calculate(command.Quantity * command.EntryPrice);
        portfolio.DeductFunds(totalCost, "Buy", command.Symbol, command.Quantity, command.EntryPrice, commission);
        await _positionRepository.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result> VerifySubscriptionLimitAsync(string traderId)
    {
        var canAdd = await _limitGuard.CanAddAssetAsync(traderId);
        return canAdd
            ? Result.Success()
            : Result.Failure("Subscription limit reached: maximum active assets exceeded.");
    }

    private async Task<Portfolio?> GetActivePortfolioAsync(string traderId)
    {
        var trader = await _traderRepository.GetByIdIncludePlanAndPositionsAsync(traderId);
        return trader?.Portfolios.FirstOrDefault(p => p.IsActive);
    }

    private decimal CalculateTotalCost(BuyPositionCommand command)
    {
        var subtotal = command.Quantity * command.EntryPrice;
        return subtotal + _commissionService.Calculate(subtotal);
    }

    private static Result EnsureSufficientBalance(decimal totalCost, decimal balance)
    {
        return totalCost > balance
            ? Result.Failure($"Insufficient balance. Required: ${totalCost:F2}, Available: ${balance:F2}")
            : Result.Success();
    }

    private async Task UpsertPositionAsync(Portfolio portfolio, BuyPositionCommand command)
    {
        var existing = portfolio.Positions.FirstOrDefault(p => p.Symbol == command.Symbol);
        if (existing != null)
        {
            existing.Update(command.Quantity, command.EntryPrice);
            return;
        }

        var position = new Position(
            Guid.NewGuid(), command.Symbol, command.Quantity, command.EntryPrice, portfolio.Id);
        await _positionRepository.AddAsync(position);
    }
}
