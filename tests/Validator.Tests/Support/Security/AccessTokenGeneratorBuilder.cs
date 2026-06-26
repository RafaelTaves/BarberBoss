namespace Validator.Tests.Support.Security;

public class AccessTokenGeneratorBuilder
{
    private string _token = "access-token";

    public AccessTokenGeneratorBuilder WithToken(string token)
    {
        _token = token;
        return this;
    }

    public AccessTokenGeneratorDouble Build()
    {
        return new AccessTokenGeneratorDouble(_token);
    }
}
