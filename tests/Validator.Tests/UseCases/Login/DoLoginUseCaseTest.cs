using BarberBoss.Application.UseCases.Login;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;
using Validator.Tests.Support.Security;

namespace Validator.Tests.UseCases.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Execute_ValidCredentials_ReturnsUserNameAndToken()
    {
        // Arrange
        var expectedToken = "login-token";
        var request = new RequestLoginJsonBuilder().Build();
        var user = new UserBuilder()
            .WithEmail(request.Email)
            .WithPasswordHash("hashed-password")
            .Build();
        var repository = new UserReadOnlyRepositoryBuilder()
            .WithUser(user)
            .Build();
        var passwordEncripter = new PasswordEncripterBuilder()
            .WithPasswordMatches(true)
            .Build();
        var accessTokenGenerator = new AccessTokenGeneratorBuilder()
            .WithToken(expectedToken)
            .Build();
        var useCase = new DoLoginUseCase(repository, passwordEncripter, accessTokenGenerator);

        // Act
        var response = await useCase.Execute(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(user.Name, response.Name);
        Assert.Equal(expectedToken, response.Token);
        Assert.Equal(request.Password, passwordEncripter.PasswordToVerify);
        Assert.Equal(user.PasswordHash, passwordEncripter.PasswordHashToVerify);
        Assert.Same(user, accessTokenGenerator.UserToGenerateToken);
    }

    [Fact]
    public async Task Execute_EmailDoesNotExist_ThrowsInvalidLoginException()
    {
        // Arrange
        var request = new RequestLoginJsonBuilder().Build();
        var repository = new UserReadOnlyRepositoryBuilder().Build();
        var useCase = new DoLoginUseCase(
            repository,
            new PasswordEncripterBuilder().Build(),
            new AccessTokenGeneratorBuilder().Build());

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<InvalidLoginException>(act);
    }

    [Fact]
    public async Task Execute_InvalidPassword_ThrowsInvalidLoginException()
    {
        // Arrange
        var request = new RequestLoginJsonBuilder().Build();
        var user = new UserBuilder()
            .WithEmail(request.Email)
            .Build();
        var repository = new UserReadOnlyRepositoryBuilder()
            .WithUser(user)
            .Build();
        var passwordEncripter = new PasswordEncripterBuilder()
            .WithPasswordMatches(false)
            .Build();
        var accessTokenGenerator = new AccessTokenGeneratorBuilder().Build();
        var useCase = new DoLoginUseCase(repository, passwordEncripter, accessTokenGenerator);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<InvalidLoginException>(act);
        Assert.Null(accessTokenGenerator.UserToGenerateToken);
    }
}
