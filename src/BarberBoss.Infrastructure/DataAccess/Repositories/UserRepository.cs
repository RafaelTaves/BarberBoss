using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserWriteOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;

    public UserRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.Set<User>().AddAsync(user);
    }
}
