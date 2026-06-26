using BarberBoss.Communication.Requests.Login;
using Bogus;

namespace Validator.Tests.Support.Builders;

public class RequestLoginJsonBuilder
{
    private readonly RequestLoginJson _request;

    public RequestLoginJsonBuilder()
    {
        _request = new Faker<RequestLoginJson>("pt_BR")
            .RuleFor(login => login.Email, faker => faker.Internet.Email())
            .RuleFor(login => login.Password, _ => "Abc123!")
            .Generate();
    }

    public RequestLoginJsonBuilder WithEmail(string email)
    {
        _request.Email = email;
        return this;
    }

    public RequestLoginJsonBuilder WithPassword(string password)
    {
        _request.Password = password;
        return this;
    }

    public RequestLoginJson Build()
    {
        return _request;
    }
}
