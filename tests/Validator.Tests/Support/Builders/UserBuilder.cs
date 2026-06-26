using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;

namespace Validator.Tests.Support.Builders;

public class UserBuilder
{
    private readonly User _user;

    public UserBuilder()
    {
        _user = new Faker<User>("pt_BR")
            .RuleFor(user => user.Id, _ => Guid.NewGuid())
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.PasswordHash, _ => "encrypted-password")
            .RuleFor(user => user.Role, _ => UserRole.Client)
            .RuleFor(user => user.CreatedAt, _ => DateTime.UtcNow)
            .RuleFor(user => user.UpdatedAt, _ => DateTime.UtcNow)
            .Generate();
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _user.PasswordHash = passwordHash;
        return this;
    }

    public User Build()
    {
        return _user;
    }
}
