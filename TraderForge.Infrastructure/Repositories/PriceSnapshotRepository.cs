using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Repositories;
using TraderForge.Infrastructure.Persistence;

namespace TraderForge.Infrastructure.Repositories;

public class PriceSnapshotRepository : IPriceSnapshotRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PriceSnapshotRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(PriceSnapshot snapshot)
    {
        await _dbContext.PriceSnapshots.AddAsync(snapshot);
        await SaveChangesAsync();
    }

    public async Task<List<PriceSnapshot>> GetBySymbolAsync(string symbol, DateTime from, DateTime to)
    {
        return await _dbContext.PriceSnapshots
            .Where(s => s.Symbol == symbol && s.RecordedAt >= from && s.RecordedAt <= to)
            .OrderBy(s => s.RecordedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
