namespace TraderForge.Domain.Interfaces;

public interface IIdentityService
{
    Task<string> RegisterUserAndReturnIdAsync(string email, string password);
    Task<string> LoginUserAndReturnJwtTokenAsync(string email, string password);
    

}