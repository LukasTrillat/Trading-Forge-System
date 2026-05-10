using System.Reflection;
using Moq;
using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Application.Handlers;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Domain.Services;

namespace TraderForge.Application.Tests;

public class SellPositionCommandHandlerTests
{
    private readonly Mock<IPositionRepository> _positionRepositoryMock;
    private readonly Mock<ICommissionService> _commissionServiceMock;
    private readonly SellPositionCommandHandler _handler;
    private readonly Position _position;
    private readonly Portfolio _portfolio;
    private static readonly Guid PositionId = Guid.NewGuid();
    private const decimal InitialBalance = 10000m;

    public SellPositionCommandHandlerTests()
    {
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _commissionServiceMock = new Mock<ICommissionService>();

        _portfolio = new Portfolio("trader-1", InitialBalance);
        _position = new Position(PositionId, "BTC", 1m, 50000m, _portfolio.Id);
        _portfolio.Positions.Add(_position);

        var backingField = typeof(Position)
            .GetField("<Portfolio>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        backingField?.SetValue(_position, _portfolio);

        _positionRepositoryMock
            .Setup(x => x.GetByIdWithPortfolioAsync(PositionId))
            .ReturnsAsync(_position);

        _handler = new SellPositionCommandHandler(
            _positionRepositoryMock.Object,
            _commissionServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenPositionFound_Should_AddFunds_And_Remove_Position()
    {
        var proceeds = 1m * 50000m;
        var commission = 250m;
        _commissionServiceMock.Setup(x => x.Calculate(proceeds)).Returns(commission);

        var command = new SellPositionCommand { PositionId = PositionId };

        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        _positionRepositoryMock.Verify(x => x.RemoveAsync(_position), Times.Once);
        Assert.Equal(InitialBalance + proceeds - commission, _portfolio.VirtualBalance);
        var transaction = Assert.Single(_portfolio.Transactions);
        Assert.Equal("Sell", transaction.Type);
        Assert.Equal("BTC", transaction.Symbol);
        Assert.Equal(1m, transaction.Quantity);
        Assert.Equal(50000m, transaction.Price);
        Assert.Equal(commission, transaction.Commission);
    }

    [Fact]
    public async Task HandleAsync_WhenPositionNotFound_Should_Return_Failure()
    {
        _positionRepositoryMock
            .Setup(x => x.GetByIdWithPortfolioAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Position?)null);

        var command = new SellPositionCommand { PositionId = Guid.NewGuid() };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryThrows_Should_Catch_Exception()
    {
        _commissionServiceMock.Setup(x => x.Calculate(It.IsAny<decimal>())).Returns(0m);
        _positionRepositoryMock
            .Setup(x => x.RemoveAsync(It.IsAny<Position>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var command = new SellPositionCommand { PositionId = PositionId };

        var result = await _handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
    }
}
