using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TraderForge.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System;
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
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(newApplicationUser, password);
        EnsureSuccessOrThrow(result);
    }

    private void EnsureSuccessOrThrow(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Unknown registration error";
            throw new Exception($"User registration failed: {errorMessage}");
        }
    }

    public async Task<string> GetValidatedTokenAsync(string email, string password)
    {
        Account user = await GetApplicationUserByEmail(email, password);
        if (await IsUserValidated(user,password))
        {
            return GenerateJwtTokenForUser(user);
        }
        else
        {
            throw new Exception("Invalid Credentials, user not validated");
        }

    }

    private async Task<Account> GetApplicationUserByEmail(string email, string password)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to find User by it's email: {ex.Message}");
        }
    }
    
    private async Task<bool> IsUserValidated(Account user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    private string GenerateJwtTokenForUser(Account user)
    {
        string secret = _jwtConfiguration["JwtSettings:Secret"] ?? throw new Exception("JWT Secret is missing!");
        string issuer = _jwtConfiguration["JwtSettings:Issuer"] ?? throw new Exception("JWT Issuer is missing!");
        string audience = _jwtConfiguration["JwtSettings:Audience"] ?? throw new Exception("JWT Audience is missing!");

        byte[] keyBytes = Encoding.UTF8.GetBytes(secret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };
            
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), 
            Expires = DateTime.UtcNow.AddHours(2), 
            Issuer = issuer,                      
            Audience = audience,                 
            SigningCredentials = credentials    
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }


}