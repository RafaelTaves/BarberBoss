using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;

namespace Validator.Tests.Support.Repositories;

public class UserUpdateOnlyRepositoryDouble : IUserUpdateOnlyRepository
{
    public User? UpdatedUser { get; private set; }

    public void Update(User user)
    {
        UpdatedUser = user;
    }
}
