using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Repositories;

public interface ITraderRepository
{
    Task AddAsync(Trader trader);

    Task<Trader> GetByIdAsync(string id);
    
    Task<Trader> GetByIdWithSubscriptionPlanAsync(string id);
    
    Task<Trader> GetByIdWithPortfoliosAsync(string id);

    Task<Trader> GetByIdWithAllAsync(string id);

    Task<List<Trader>> GetExpiredTrialsAsync();
    
    Task SaveChangesAsync();
}