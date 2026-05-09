using TraderForge.Application.Common;
using TraderForge.Application.DTOs;
using TraderForge.Domain.Entities;
using TraderForge.Domain.Interfaces;

namespace TraderForge.Application.Handlers;

public class GetAllPlansQueryHandler
{
    private readonly ISubscriptionPlanRepository _planRepository;
    public GetAllPlansQueryHandler(ISubscriptionPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }
    public async Task<Result<List<SubscriptionPlan>>> GetAllSubscriptionPlans(GetAllPlansQuery query)
    {
        try
        {
            var plans = await _planRepository.GetAllAsync();
            return Result<List<SubscriptionPlan>>.Success(plans.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<SubscriptionPlan>>.Failure(ex.Message);
        }
    }
}
