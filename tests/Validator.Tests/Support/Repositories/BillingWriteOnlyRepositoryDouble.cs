using BarberBoss.Domain.Repositories.Billings;

namespace Validator.Tests.Support.Repositories;

public class BillingWriteOnlyRepositoryDouble : IBillingWriteOnlyRepository
{
    private readonly bool _deleteResult;

    public BillingWriteOnlyRepositoryDouble(bool deleteResult)
    {
        _deleteResult = deleteResult;
    }

    public BarberBoss.Domain.Entities.Billing? AddedBilling { get; private set; }
    public Guid? DeletedId { get; private set; }
    public Guid? DeletedAllByUserId { get; private set; }

    public Task Add(BarberBoss.Domain.Entities.Billing billing)
    {
        AddedBilling = billing;
        return Task.CompletedTask;
    }

    public Task<bool> Delete(Guid id)
    {
        DeletedId = id;
        return Task.FromResult(_deleteResult);
    }

    public Task DeleteAllByUserId(Guid userId)
    {
        DeletedAllByUserId = userId;
        return Task.CompletedTask;
    }
}
