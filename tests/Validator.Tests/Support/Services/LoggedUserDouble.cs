using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Services.LoggedUser;

namespace Validator.Tests.Support.Services;

public class LoggedUserDouble : ILoggedUser
{
    private readonly User _user;

    public LoggedUserDouble(User user)
    {
        _user = user;
    }

    public Task<User> Get()
    {
        return Task.FromResult(_user);
    }
}
