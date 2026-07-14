using BarberBoss.Application.UseCases.User.Delete;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.Repositories;

namespace Validator.Tests.UseCases.User.Delete;

public class DeleteUserByAdminUseCaseTest
{
    [Fact]
    public async Task Execute_UserExists_DeletesBillingsCascadeAndUserAndCommits()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRepository = new UserWriteOnlyRepositoryBuilder()
            .WithDeleteResult(true)
            .Build();
        var billingRepository = new BillingWriteOnlyRepositoryBuilder()
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteUserByAdminUseCase(
            userRepository,
            billingRepository,
            unitOfWork);

        // Act
        await useCase.Execute(userId);

        // Assert
        Assert.Equal(userId, billingRepository.DeletedAllByUserId);
        Assert.Equal(userId, userRepository.DeletedId);
        Assert.Equal(1, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_UserDoesNotExist_ThrowsNotFoundExceptionAndDoesNotCommit()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRepository = new UserWriteOnlyRepositoryBuilder()
            .WithDeleteResult(false)
            .Build();
        var billingRepository = new BillingWriteOnlyRepositoryBuilder()
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteUserByAdminUseCase(
            userRepository,
            billingRepository,
            unitOfWork);

        // Act
        var act = async () => await useCase.Execute(userId);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(userId, billingRepository.DeletedAllByUserId);
        Assert.Equal(userId, userRepository.DeletedId);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
