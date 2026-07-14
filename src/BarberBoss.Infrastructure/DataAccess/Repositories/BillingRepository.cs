using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingRepository : IBillingReadOnlyRepository, IBillingWriteOnlyRepository, IBillingUpdateOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;

    public BillingRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task<Billing?> IBillingReadOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Billings.AsNoTracking().FirstOrDefaultAsync(billing => billing.Id == id);
    }

    async Task<Billing?> IBillingUpdateOnlyRepository.GetById(Guid id)
    {
        return await _dbContext.Billings.FirstOrDefaultAsync(billing => billing.Id == id);
    }

    public async Task<List<Billing>> GetAll()
    {
        return await _dbContext.Billings.AsNoTracking().ToListAsync();
    }

    public async Task Add(Billing billing)
    {
        await _dbContext.Billings.AddAsync(billing);
    }

    public void Update(Billing billing)
    {
        _dbContext.Billings.Update(billing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _dbContext.Billings.FirstOrDefaultAsync(Billing => Billing.Id == id);
        if(result is null)
        {
            return false;
        }

        _dbContext.Billings.Remove(result);

        return true;
    }

    public async Task<List<Billing>> FilterByMonth(DateOnly date)
    {
        return await _dbContext.Billings
            .AsNoTracking()
            .Where(b => b.Date.Year == date.Year && b.Date.Month == date.Month)
            .OrderBy(billing => billing.Date)
            .ToListAsync();
    }

    public async Task DeleteAllByUserId(Guid userId)
    {
        var billings = await _dbContext.Billings.Where(billing => billing.UserId == userId).ToListAsync();

        _dbContext.Billings.RemoveRange(billings);
    }
}
