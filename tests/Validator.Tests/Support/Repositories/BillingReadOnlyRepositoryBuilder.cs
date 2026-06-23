namespace Validator.Tests.Support.Repositories;

public class BillingReadOnlyRepositoryBuilder
{
    private readonly List<BarberBoss.Domain.Entities.Billing> _billings = [];

    public BillingReadOnlyRepositoryBuilder WithBilling(BarberBoss.Domain.Entities.Billing billing)
    {
        _billings.Add(billing);
        return this;
    }

    public BillingReadOnlyRepositoryBuilder WithBillings(List<BarberBoss.Domain.Entities.Billing> billings)
    {
        _billings.AddRange(billings);
        return this;
    }

    public BillingReadOnlyRepositoryDouble Build()
    {
        return new BillingReadOnlyRepositoryDouble(_billings);
    }
}
