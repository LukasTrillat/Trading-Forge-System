using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TraderForge.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System;

namespace TraderForge.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _jwtConfiguration;

    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration jwtConfiguration)
    {
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    }

    public async Task<string> RegisterUserAndReturnIdAsync(string email, string password)
    {
        var user = new ApplicationUser()
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Unknown registration error";
            throw new Exception($"User registration failed: {errorMessage}");
        }

        return user.Id;
    }

    public async Task<string> LoginUserAndReturnJwtTokenAsync(string email, string password)
    {
        ApplicationUser user = await GetApplicationUserByEmail(email, password);
        if (await IsUserValidated(user,password))
        {
            return GenerateJwtTokenForUser(user);
        }
        else
        {
            throw new Exception("Invalid Credentials");
        }

    }

    private async Task<ApplicationUser> GetApplicationUserByEmail(string email, string password)
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
    
    private async Task<bool> IsUserValidated(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    private string GenerateJwtTokenForUser(ApplicationUser user)
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