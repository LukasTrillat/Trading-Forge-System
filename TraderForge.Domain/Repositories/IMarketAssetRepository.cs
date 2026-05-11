using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Repositories;

public interface IMarketAssetRepository
{
    Task<IEnumerable<MarketAsset>> GetAllAsync();
    Task AddAsync(MarketAsset asset);
    Task UpdateAsync(MarketAsset asset);
    Task SaveChangesAsync();
}
