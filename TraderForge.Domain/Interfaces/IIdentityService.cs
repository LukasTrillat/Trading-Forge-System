namespace TraderForge.Domain.Interfaces;

public interface IIdentityService
{
    Task<string> RegisterUserAndReturnIdAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);

}