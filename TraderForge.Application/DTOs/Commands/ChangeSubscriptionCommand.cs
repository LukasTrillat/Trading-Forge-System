namespace TraderForge.Application.DTOs;

public class ChangeSubscriptionCommand
{
    public string TraderId { get; set; }
    public Guid NewPlanId { get; set; }
}