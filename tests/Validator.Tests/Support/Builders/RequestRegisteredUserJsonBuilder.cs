using BarberBoss.Communication.Requests.User;
using Bogus;

namespace Validator.Tests.Support.Builders;

public class RequestRegisteredUserJsonBuilder
{
    private readonly RequestRegisteredUserJson _request;

    public RequestRegisteredUserJsonBuilder()
    {
        _request = new Faker<RequestRegisteredUserJson>("pt_BR")
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, _ => "Abc123!")
            .Generate();
    }

    public RequestRegisteredUserJsonBuilder WithName(string name)
    {
        _request.Name = name;
        return this;
    }

    public RequestRegisteredUserJsonBuilder WithEmail(string email)
    {
        _request.Email = email;
        return this;
    }

    public RequestRegisteredUserJsonBuilder WithPassword(string password)
    {
        _request.Password = password;
        return this;
    }

    public RequestRegisteredUserJson Build()
    {
        return _request;
    }
}
