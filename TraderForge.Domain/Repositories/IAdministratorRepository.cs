using TraderForge.Domain.Entities;

namespace TraderForge.Domain.Interfaces;

public interface IAdministratorRepository
{
   Task AddAsync(Administrator administrator);
   Task<Administrator>? GetAdministratorByIdAsync(string id);
   Task SaveChangesAsync();
}