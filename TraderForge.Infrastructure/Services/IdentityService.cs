using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TraderForge.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TraderForge.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Account> _userManager;
    private readonly IConfiguration _jwtConfiguration;

    public IdentityService(UserManager<Account> userManager, IConfiguration jwtConfiguration)
    {
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    }

    public async Task RegisterNewAccountAsync(string newUserId, string email, string password)
    {
        var newApplicationUser = new Account()
        {
            Id = newUserId,
            UserName = email, // ASP.NET Identity uses UserName for login
            Email = email
        };

        // This method handles the password hashing automatically
        var result = await _userManager.CreateAsync(newApplicationUser, password);
        
        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault()?.Description ?? "Registration failed";
            throw new Exception(error);
        }

        // Assign the default role
        await _userManager.AddClaimAsync(newApplicationUser, new Claim(ClaimTypes.Role, "Trader"));
    }

    public async Task<string> GetValidatedTokenAsync(string email, string password)
    {
        // 1. Find user by email (or username since they are same)
        var user = await _userManager.FindByEmailAsync(email) 
                   ?? await _userManager.FindByNameAsync(email);

        if (user == null)
        {
            throw new Exception("Invalid Credentials: User does not exist.");
        }

        // 2. Verify the hashed password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        
        if (isPasswordValid)
        {
            return await GenerateJwtTokenForUserAsync(user);
        }

        throw new Exception("Invalid Credentials: Password is incorrect.");
    }

    // ... Keep your existing GenerateJwtTokenForUserAsync and helper methods below ...
    private async Task<string> GenerateJwtTokenForUserAsync(Account user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = await GenerateSecurityTokenDescriptorAsync(user);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<SecurityTokenDescriptor> GenerateSecurityTokenDescriptorAsync(Account user)
    {
        string secret = _jwtConfiguration["JwtSettings:Secret"] ?? "YourDefaultSecretForDevelopmentOnly!!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };
        
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _jwtConfiguration["JwtSettings:Issuer"],
            Audience = _jwtConfiguration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };
    }
}
