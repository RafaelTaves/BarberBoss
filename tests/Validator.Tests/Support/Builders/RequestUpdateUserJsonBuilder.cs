using BarberBoss.Communication.Requests.User;
using Bogus;

namespace Validator.Tests.Support.Builders;

public class RequestUpdateUserJsonBuilder
{
    private readonly RequestUpdateUserJson _request;

    public RequestUpdateUserJsonBuilder()
    {
        _request = new Faker<RequestUpdateUserJson>("pt_BR")
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .Generate();
    }

    public RequestUpdateUserJsonBuilder WithName(string name)
    {
        _request.Name = name;
        return this;
    }

    public RequestUpdateUserJsonBuilder WithEmail(string email)
    {
        _request.Email = email;
        return this;
    }

    public RequestUpdateUserJson Build()
    {
        return _request;
    }
}
