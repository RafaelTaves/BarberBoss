using BarberBoss.Domain.Repositories.Users;

namespace Validator.Tests.Support.Repositories;

public class UserReadOnlyRepositoryDouble : IUserReadOnlyRepository
{
    private readonly List<BarberBoss.Domain.Entities.User> _users;

    public UserReadOnlyRepositoryDouble(List<BarberBoss.Domain.Entities.User> users)
    {
        _users = users;
    }

    public Task<bool> ExistActiveUserWithEmail(string email)
    {
        var exists = _users.Any(user => user.Email == email);

        return Task.FromResult(exists);
    }

    public Task<BarberBoss.Domain.Entities.User?> GetUserByEmail(string email)
    {
        var user = _users.FirstOrDefault(user => user.Email == email);

        return Task.FromResult(user);
    }
}
