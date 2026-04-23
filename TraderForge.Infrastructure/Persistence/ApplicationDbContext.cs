using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;
namespace TraderForge.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<MarketAsset> MarketAssets { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure database constraints
    }
}