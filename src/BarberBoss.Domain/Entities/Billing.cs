using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities;

public class Billing
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public  BillingPaymentMethod PaymentMethod { get; set; }
    public BillingStatus Status { get; set; } 
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
