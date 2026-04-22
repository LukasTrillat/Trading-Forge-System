using Microsoft.AspNetCore.Identity;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;
using TraderForge.Infrastructure.Persistence;

namespace TraderForge.Infrastructure.Repositories;

public class TraderRepository : ITraderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TraderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Trader trader)
    {
        await _dbContext.Traders.AddAsync(trader);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}