using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq; // Added for .Any()
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TraderForge.Infrastructure.Persistence; // Assuming Account lives here or similar

namespace TraderForge.Infrastructure.Services;

public class AdminConfig
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AdminSeederService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public AdminSeederService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var adminCredentials = GetAdminCredentials(configuration);

        foreach (var adminConfig in adminCredentials)
        {
            if (string.IsNullOrWhiteSpace(adminConfig.Email) || string.IsNullOrWhiteSpace(adminConfig.Password))
            {
                continue;
            }

            // Fetch the user ONCE
            var user = await userManager.FindByEmailAsync(adminConfig.Email);

            if (user != null)
            {
                // User exists: Ensure password is correct and claims are assigned
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, resetToken, adminConfig.Password);

                var claims = await userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "SystemAdmin"))
                {
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "SystemAdmin"));
                }
            }
            else
            {
                // User does not exist: Create new
                await RegisterNewAdmin(userManager, adminConfig);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // Made synchronous since there are no await calls inside
    private List<AdminConfig> GetAdminCredentials(IConfiguration configuration)
    {
        var admins = configuration.GetSection("Admins").Get<List<AdminConfig>>();
        
        if (admins == null || !admins.Any())
        {
            throw new Exception("No admins registered in configuration.");
        }
        
        return admins;
    }

    private async Task RegisterNewAdmin(UserManager<Account> userManager, AdminConfig adminConfig)
    {
        var user = new Account
        {
            UserName = adminConfig.Email,
            Email = adminConfig.Email
        };

        var result = await userManager.CreateAsync(user, adminConfig.Password);
        if (result.Succeeded)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "SystemAdmin"));
        }
    }
}
