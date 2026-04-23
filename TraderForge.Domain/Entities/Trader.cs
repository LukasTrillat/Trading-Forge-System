namespace TraderForge.Domain.Entities;

public class Trader
{
    public string Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    
    public DateTime FreeTrialExpirationDate{ get; private set; }
    public DateTime FreeTrialRegistrationDate { get; private set; }
    
    public Guid? SubscriptionPlanId { get; private set; }
    public SubscriptionPlan SubscriptionPlan { get; private set; }

    public ICollection<Portfolio> Portfolios { get; private set; } = new List<Portfolio>();

    public Trader(string id, string email)
    {
        Id = id;
        Email = email;
    }

    public void AssignSubscriptionPlan(SubscriptionPlan plan)
    {
        SubscriptionPlanId = plan.Id;
        SubscriptionPlan = plan;
    }
}