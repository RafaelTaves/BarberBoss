using BarberBoss.Application.UseCases.Billing.Delete;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.Repositories;

namespace Validator.Tests.UseCases.Billing.Delete;

public class DeleteBillingUseCaseTest
{
    [Fact]
    public async Task Execute_BillingExists_DeletesBillingAndCommits()
    {
        // Arrange
        var billingId = Guid.NewGuid();
        var repository = new BillingWriteOnlyRepositoryBuilder()
            .WithDeleteResult(true)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteBillingUseCase(repository, unitOfWork);

        // Act
        await useCase.Execute(billingId);

        // Assert
        Assert.Equal(billingId, repository.DeletedId);
        Assert.Equal(1, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_BillingDoesNotExist_ThrowsNotFoundExceptionAndDoesNotCommit()
    {
        // Arrange
        var repository = new BillingWriteOnlyRepositoryBuilder()
            .WithDeleteResult(false)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new DeleteBillingUseCase(repository, unitOfWork);

        // Act
        var act = async () => await useCase.Execute(Guid.NewGuid());

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
