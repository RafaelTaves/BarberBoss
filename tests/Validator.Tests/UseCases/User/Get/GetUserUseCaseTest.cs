using BarberBoss.Application.UseCases.User.Get;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Services;

namespace Validator.Tests.UseCases.User.Get;

public class GetUserUseCaseTest
{
    [Fact]
    public async Task Execute_LoggedUserExists_ReturnsUser()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var loggedUser = new LoggedUserDouble(user);
        var useCase = new GetUserUseCase(loggedUser, TestMapper.Create());

        // Act
        var response = await useCase.Execute();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(user.Id, response.Id);
        Assert.Equal(user.Name, response.Name);
        Assert.Equal(user.Email, response.Email);
        Assert.Equal(user.Role, response.Role);
    }
}
