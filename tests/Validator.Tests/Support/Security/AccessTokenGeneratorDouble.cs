using BarberBoss.Domain.Security.Tokens;

namespace Validator.Tests.Support.Security;

public class AccessTokenGeneratorDouble : IAccessTokenGenerator
{
    private readonly string _token;

    public AccessTokenGeneratorDouble(string token)
    {
        _token = token;
    }

    public BarberBoss.Domain.Entities.User? UserToGenerateToken { get; private set; }

    public string Generate(BarberBoss.Domain.Entities.User user)
    {
        UserToGenerateToken = user;

        return _token;
    }
}
