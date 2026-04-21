using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraderForge.Domain.Entities;

namespace TraderForge.Infrastructure.Persistence;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Trader> Traders { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure database constraints
    }
}