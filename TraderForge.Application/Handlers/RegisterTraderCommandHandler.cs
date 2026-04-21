using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class RegisterTraderCommandHandler
{
    public IIdentityService IdentityService;

    RegisterTraderCommandHandler(IIdentityService identityService)
    {
        IdentityService = identityService;
    }
}