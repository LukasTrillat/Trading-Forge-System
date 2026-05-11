using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TraderForge.Infrastructure.Repositories;

public class MarketAssetRepository : IMarketAssetRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MarketAssetRepository(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<IEnumerable<MarketAsset>> GetAllAsync()
    {
        return await _dbContext.MarketAssets.ToListAsync();
    }

    public async Task AddAsync(MarketAsset asset)
    {
        await _dbContext.MarketAssets.AddAsync(asset);
    }

    public async Task UpdateAsync(MarketAsset asset)
    {
        _dbContext.MarketAssets.Update(asset);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
