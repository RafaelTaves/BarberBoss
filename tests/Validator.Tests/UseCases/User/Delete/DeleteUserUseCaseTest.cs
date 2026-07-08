using BarberBoss.Application.UseCases.User.Delete;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;
using Validator.Tests.Support.Services;

namespace Validator.Tests.UseCases.User.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Execute_LoggedUserExists_DeletesUserAndCommits()
    {
        // Arrange
        var loggedUser = new UserBuilder().Build();
        var repository = new UserWriteOnlyRepositoryBuilder()
            .WithDeleteResult(true)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteUserUseCase(
            repository,
            new LoggedUserDouble(loggedUser),
            unitOfWork);

        // Act
        await useCase.Execute();

        // Assert
        Assert.Equal(loggedUser.Id, repository.DeletedId);
        Assert.Equal(1, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_UserDoesNotExist_ThrowsNotFoundExceptionAndDoesNotCommit()
    {
        // Arrange
        var loggedUser = new UserBuilder().Build();
        var repository = new UserWriteOnlyRepositoryBuilder()
            .WithDeleteResult(false)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteUserUseCase(
            repository,
            new LoggedUserDouble(loggedUser),
            unitOfWork);

        // Act
        var act = async () => await useCase.Execute();

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(loggedUser.Id, repository.DeletedId);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
