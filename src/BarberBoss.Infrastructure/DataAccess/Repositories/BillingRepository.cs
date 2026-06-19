using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingRepository : IBillingReadOnlyRepository, IBillingUpdateOnlyRepository, IBillingWriteOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;

    public BillingRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Billing billing)
    {
        _dbContext.Billings.Add(billing);

        _dbContext.SaveChanges();
    }
}
