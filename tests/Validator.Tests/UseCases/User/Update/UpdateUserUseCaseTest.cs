using BarberBoss.Application.UseCases.User.Update;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;
using Validator.Tests.Support.Services;

namespace Validator.Tests.UseCases.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Execute_ValidRequest_UpdatesUserAndCommits()
    {
        // Arrange
        var loggedUser = new UserBuilder().Build();
        var request = new RequestUpdateUserJsonBuilder()
            .WithName("Rafael Santos")
            .WithEmail("rafael@email.com")
            .Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder()
            .WithUser(loggedUser)
            .Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateUserUseCase(
            readOnlyRepository,
            updateOnlyRepository,
            new LoggedUserDouble(loggedUser),
            unitOfWork,
            TestMapper.Create());

        // Act
        await useCase.Execute(request);

        // Assert
        Assert.NotNull(updateOnlyRepository.UpdatedUser);
        Assert.Equal(loggedUser.Id, updateOnlyRepository.UpdatedUser.Id);
        Assert.Equal(request.Name, updateOnlyRepository.UpdatedUser.Name);
        Assert.Equal(request.Email, updateOnlyRepository.UpdatedUser.Email);
        Assert.Equal(1, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_InvalidRequest_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var loggedUser = new UserBuilder().Build();
        var request = new RequestUpdateUserJsonBuilder()
            .WithName(string.Empty)
            .Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateUserUseCase(
            readOnlyRepository,
            updateOnlyRepository,
            new LoggedUserDouble(loggedUser),
            unitOfWork,
            TestMapper.Create());

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Null(updateOnlyRepository.UpdatedUser);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_EmailBelongsToAnotherUser_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var loggedUser = new UserBuilder().Build();
        var anotherUser = new UserBuilder().WithEmail("already-used@email.com").Build();
        var request = new RequestUpdateUserJsonBuilder()
            .WithEmail(anotherUser.Email)
            .Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder()
            .WithUser(anotherUser)
            .Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateUserUseCase(
            readOnlyRepository,
            updateOnlyRepository,
            new LoggedUserDouble(loggedUser),
            unitOfWork,
            TestMapper.Create());

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Null(updateOnlyRepository.UpdatedUser);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
