using Microsoft.AspNetCore.Identity;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    
    
}