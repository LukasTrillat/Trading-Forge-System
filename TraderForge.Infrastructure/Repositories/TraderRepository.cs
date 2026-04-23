using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;
using TraderForge.Infrastructure.Persistence;
using System.Linq;

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

    public async Task<Trader> GetByIdAsync(string id)
    {
        return await _dbContext.Traders.Include(t => t.Portfolios).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}