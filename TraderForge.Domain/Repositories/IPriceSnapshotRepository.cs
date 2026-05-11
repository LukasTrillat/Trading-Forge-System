using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Repositories;

public interface IPriceSnapshotRepository
{
    Task AddAsync(PriceSnapshot snapshot);
    Task<List<PriceSnapshot>> GetBySymbolAsync(string symbol, DateTime from, DateTime to);
    Task SaveChangesAsync();
}
