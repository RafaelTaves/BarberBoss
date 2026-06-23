using BarberBoss.Domain.Enums;

namespace Validator.Tests.Support.Builders;

public class BillingBuilder
{
    private readonly BarberBoss.Domain.Entities.Billing _billing = new()
    {
        Id = Guid.NewGuid(),
        Date = new DateOnly(2026, 6, 23),
        BarberName = "Rafael",
        ClientName = "Joao",
        ServiceName = "Corte",
        Amount = 50,
        PaymentMethod = BillingPaymentMethod.Pix,
        Status = BillingStatus.Pago,
        Notes = "Cliente recorrente",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    public BillingBuilder WithId(Guid id)
    {
        _billing.Id = id;
        return this;
    }

    public BarberBoss.Domain.Entities.Billing Build()
    {
        return _billing;
    }
}
