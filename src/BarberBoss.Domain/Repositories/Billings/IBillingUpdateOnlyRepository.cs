using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingUpdateOnlyRepository
{
    Task<Billing?> GetById(long id);
    void Update(Billing billing);
}
