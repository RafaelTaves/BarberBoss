namespace BarberBoss.Communication.Responses.Billing;

public class ResponseShortBillingJson
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
}