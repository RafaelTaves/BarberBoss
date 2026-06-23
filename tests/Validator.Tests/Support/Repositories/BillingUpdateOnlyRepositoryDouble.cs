using BarberBoss.Domain.Repositories.Billings;

namespace Validator.Tests.Support.Repositories;

public class BillingUpdateOnlyRepositoryDouble : IBillingUpdateOnlyRepository
{
    private readonly BarberBoss.Domain.Entities.Billing? _billing;

    public BillingUpdateOnlyRepositoryDouble(BarberBoss.Domain.Entities.Billing? billing)
    {
        _billing = billing;
    }

    public BarberBoss.Domain.Entities.Billing? UpdatedBilling { get; private set; }

    public Task<BarberBoss.Domain.Entities.Billing?> GetById(Guid id)
    {
        return Task.FromResult(_billing is not null && _billing.Id == id ? _billing : null);
    }

    public void Update(BarberBoss.Domain.Entities.Billing billing)
    {
        UpdatedBilling = billing;
    }
}
