namespace TraderForge.Domain.Entities;

public class Trader
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime FreeTrialExpirationDate{ get; set; }
    public DateTime FreeTrialRegistrationDate { get; set; }

    public Trader(string id, string email)
    {
        Id = id;
        Email = email;
    }
}