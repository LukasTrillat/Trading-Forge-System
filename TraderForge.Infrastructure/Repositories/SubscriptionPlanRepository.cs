using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;
using TraderForge.Infrastructure.Persistence;

namespace TraderForge.Infrastructure.Repositories;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SubscriptionPlanRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<SubscriptionPlan> GetByIdAsync(Guid id)
    {
        return await _dbContext.SubscriptionPlans.FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<IEnumerable<SubscriptionPlan>> GetAllAsync()
    {
        return await _dbContext.SubscriptionPlans.ToListAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}