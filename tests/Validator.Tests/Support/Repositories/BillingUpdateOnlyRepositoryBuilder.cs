namespace Validator.Tests.Support.Repositories;

public class BillingUpdateOnlyRepositoryBuilder
{
    private BarberBoss.Domain.Entities.Billing? _billing;

    public BillingUpdateOnlyRepositoryBuilder WithBilling(BarberBoss.Domain.Entities.Billing billing)
    {
        _billing = billing;
        return this;
    }

    public BillingUpdateOnlyRepositoryDouble Build()
    {
        return new BillingUpdateOnlyRepositoryDouble(_billing);
    }
}
