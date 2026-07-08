using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
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

    public async Task<bool> Delete(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);

        return true;
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}
