using BarberBoss.Domain.Repositories.Billings;

namespace Validator.Tests.Support.Repositories;

public class BillingReadOnlyRepositoryDouble : IBillingReadOnlyRepository
{
    private readonly List<BarberBoss.Domain.Entities.Billing> _billings;

    public BillingReadOnlyRepositoryDouble(List<BarberBoss.Domain.Entities.Billing> billings)
    {
        _billings = billings;
    }

    public Task<List<BarberBoss.Domain.Entities.Billing>> GetAll()
    {
        return Task.FromResult(_billings);
    }

    public Task<BarberBoss.Domain.Entities.Billing?> GetById(Guid id)
    {
        var billing = _billings.FirstOrDefault(billing => billing.Id == id);

        return Task.FromResult(billing);
    }

    public Task<List<BarberBoss.Domain.Entities.Billing>> FilterByMonth(DateOnly date)
    {
        var billings = _billings
            .Where(billing => billing.Date.Year == date.Year && billing.Date.Month == date.Month)
            .ToList();

        return Task.FromResult(billings);
    }
}
