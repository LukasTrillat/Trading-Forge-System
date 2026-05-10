using Moq;
using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Application.Handlers;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Services;

namespace TraderForge.Application.Tests;

public class BuyPositionCommandHandlerTests
{
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<ITraderRepository> _traderRepositoryMock;
    private readonly Mock<ISubscriptionLimitGuard> _limitGuardMock;
    private readonly Mock<ICommissionService> _commissionServiceMock;
    private readonly BuyPositionCommandHandler _handler;
    private readonly Trader _trader;
    private readonly Portfolio _portfolio;
    private const string TraderId = "trader-1";
    private const decimal InitialBalance = 10000m;

    public BuyPositionCommandHandlerTests()
    {
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _traderRepositoryMock = new Mock<ITraderRepository>();
        _limitGuardMock = new Mock<ISubscriptionLimitGuard>();
        _commissionServiceMock = new Mock<ICommissionService>();

        _portfolio = new Portfolio(TraderId, InitialBalance);
        _trader = new Trader(TraderId, "test@test.com");
        _trader.Portfolios.Add(_portfolio);

        _traderRepositoryMock
            .Setup(x => x.GetByIdIncludePlanAndPositionsAsync(TraderId))
            .ReturnsAsync(_trader);

        _positionRepositoryMock
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        _handler = new BuyPositionCommandHandler(
            _positionRepositoryMock.Object,
            _traderRepositoryMock.Object,
            _limitGuardMock.Object,
            _commissionServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenNewPosition_Should_Create_Position_And_Deduct_Funds()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(true);
        _commissionServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>())).Returns(5m);

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 0.1m,
            EntryPrice = 50000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        _positionRepositoryMock.Verify(x => x.AddAsync(It.Is<Position>(p =>
            p.Symbol == "BTC" &&
            p.Quantity == 0.1m &&
            p.EntryPrice == 50000m)), Times.Once);
        Assert.Equal(InitialBalance - 5005m, _portfolio.VirtualBalance);
        Assert.Single(_portfolio.Transactions);
    }

    [Fact]
    public async Task HandleAsync_WhenExistingPosition_Should_Upsert_With_Weighted_Average()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(true);
        _commissionServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>())).Returns(10m);

        var portfolio = new Portfolio(TraderId, 200000m);
        _trader.Portfolios.Clear();
        _trader.Portfolios.Add(portfolio);

        var existing = new Position(Guid.NewGuid(), "BTC", 1m, 40000m, portfolio.Id);
        portfolio.Positions.Add(existing);

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 1m,
            EntryPrice = 60000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal(2m, existing.Quantity);
        Assert.Equal(50000m, existing.EntryPrice);
    }

    [Fact]
    public async Task HandleAsync_WhenLimitExceeded_Should_Return_Failure()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(false);

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 0.1m,
            EntryPrice = 50000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains("limit", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HandleAsync_WhenNoActivePortfolio_Should_Return_Failure()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(true);
        _portfolio.FreezeSimulation();

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 0.1m,
            EntryPrice = 50000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains("portfolio", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HandleAsync_WhenInsufficientBalance_Should_Return_Failure()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(true);
        _commissionServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>())).Returns(0m);

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 10m,
            EntryPrice = 2000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains("balance", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HandleAsync_WhenCommissionServiceFails_Should_Catch_Exception()
    {
        _limitGuardMock.Setup(x => x.CanAddAssetAsync(TraderId)).ReturnsAsync(true);
        _commissionServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>())).Throws<InvalidOperationException>();

        var command = new BuyPositionCommand
        {
            TraderId = TraderId,
            Symbol = "BTC",
            Quantity = 0.1m,
            EntryPrice = 50000m
        };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
    }
}
