namespace BarberBoss.Communication.Responses.Billing;

public class ResponseRegisteredBillingJson
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
