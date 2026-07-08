using BarberBoss.Domain.Repositories.Users;

namespace Validator.Tests.Support.Repositories;

public class UserWriteOnlyRepositoryDouble : IUserWriteOnlyRepository
{
    private readonly bool _deleteResult;

    public UserWriteOnlyRepositoryDouble(bool deleteResult)
    {
        _deleteResult = deleteResult;
    }

    public BarberBoss.Domain.Entities.User? AddedUser { get; private set; }

    public Guid? DeletedId { get; private set; }

    public Task Add(BarberBoss.Domain.Entities.User user)
    {
        AddedUser = user;

        return Task.CompletedTask;
    }

    public Task<bool> Delete(Guid id)
    {
        DeletedId = id;

        return Task.FromResult(_deleteResult);
    }
}
