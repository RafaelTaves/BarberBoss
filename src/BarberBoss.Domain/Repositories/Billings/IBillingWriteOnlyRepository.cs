using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingWriteOnlyRepository
{
    Task Add(Billing billing);
    Task<bool> Delete(Guid id);
    Task DeleteAllByUserId(Guid userId);
}
