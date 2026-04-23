using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Interfaces;

public interface ISubscriptionPlanRepository
{
    Task<SubscriptionPlan> GetByIdAsync(Guid id);
    
    Task<IEnumerable<SubscriptionPlan>> GetAllAsync();

    Task SaveChangesAsync();
}