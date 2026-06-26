using BarberBoss.Domain.Repositories.Users;

namespace Validator.Tests.Support.Repositories;

public class UserWriteOnlyRepositoryDouble : IUserWriteOnlyRepository
{
    public BarberBoss.Domain.Entities.User? AddedUser { get; private set; }

    public Task Add(BarberBoss.Domain.Entities.User user)
    {
        AddedUser = user;

        return Task.CompletedTask;
    }
}
