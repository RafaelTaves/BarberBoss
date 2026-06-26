using BarberBoss.Application.UseCases.User.Register;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;
using Validator.Tests.Support.Security;

namespace Validator.Tests.UseCases.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Execute_ValidRequest_AddsUserCommitsAndReturnsToken()
    {
        // Arrange
        var encryptedPassword = "encrypted-user-password";
        var expectedToken = "generated-user-token";
        var request = new RequestRegisteredUserJsonBuilder().Build();
        var writeOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var passwordEncripter = new PasswordEncripterBuilder()
            .WithEncryptedPassword(encryptedPassword)
            .Build();
        var accessTokenGenerator = new AccessTokenGeneratorBuilder()
            .WithToken(expectedToken)
            .Build();
        var useCase = new RegisterUserUseCase(
            writeOnlyRepository,
            readOnlyRepository,
            unitOfWork,
            TestMapper.Create(),
            passwordEncripter,
            accessTokenGenerator);

        // Act
        var response = await useCase.Execute(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(expectedToken, response.Token);
        Assert.NotNull(writeOnlyRepository.AddedUser);
        Assert.Equal(request.Name, writeOnlyRepository.AddedUser.Name);
        Assert.Equal(request.Email, writeOnlyRepository.AddedUser.Email);
        Assert.Equal(encryptedPassword, writeOnlyRepository.AddedUser.PasswordHash);
        Assert.Equal(UserRole.Client, writeOnlyRepository.AddedUser.Role);
        Assert.NotEqual(Guid.Empty, writeOnlyRepository.AddedUser.Id);
        Assert.Equal(1, unitOfWork.CommitsCount);
        Assert.Equal(request.Password, passwordEncripter.PasswordToEncrypt);
        Assert.Same(writeOnlyRepository.AddedUser, accessTokenGenerator.UserToGenerateToken);
    }

    [Fact]
    public async Task Execute_EmailAlreadyExists_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder().Build();
        var existingUser = new UserBuilder()
            .WithEmail(request.Email)
            .Build();
        var writeOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder()
            .WithUser(existingUser)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new RegisterUserUseCase(
            writeOnlyRepository,
            readOnlyRepository,
            unitOfWork,
            TestMapper.Create(),
            new PasswordEncripterBuilder().Build(),
            new AccessTokenGeneratorBuilder().Build());

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_EXISTS, exception.GetErrors());
        Assert.Null(writeOnlyRepository.AddedUser);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_InvalidRequest_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder()
            .WithPassword("invalid")
            .Build();
        var writeOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new RegisterUserUseCase(
            writeOnlyRepository,
            readOnlyRepository,
            unitOfWork,
            TestMapper.Create(),
            new PasswordEncripterBuilder().Build(),
            new AccessTokenGeneratorBuilder().Build());

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Null(writeOnlyRepository.AddedUser);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
