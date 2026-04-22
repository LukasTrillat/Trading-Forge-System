using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class LoginTraderQueryHandler
{
    
    private readonly IIdentityService _identityService;
    private readonly ITraderRepository _traderRepository;

    public LoginTraderQueryHandler(IIdentityService identityService, ITraderRepository traderRepository)
    {
        _identityService = identityService;
        _traderRepository = traderRepository;
    }


    public async Task<Result<string>> LoginAsync(LoginTraderQuery query)
    {
        try
        {
            string jwtToken = await _identityService.LoginUserAndReturnJwtTokenAsync(query.Email, query.Password);

            return Result<string>.Success(jwtToken);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

}