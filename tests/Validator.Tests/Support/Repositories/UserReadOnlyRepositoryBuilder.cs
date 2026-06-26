namespace Validator.Tests.Support.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly List<BarberBoss.Domain.Entities.User> _users = [];

    public UserReadOnlyRepositoryBuilder WithUser(BarberBoss.Domain.Entities.User user)
    {
        _users.Add(user);
        return this;
    }

    public UserReadOnlyRepositoryDouble Build()
    {
        return new UserReadOnlyRepositoryDouble(_users);
    }
}
