using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Interfaces;

public interface ITraderRepository
{
    Task AddAsync(Trader trader);
    Task SaveChangesAsync();
}