using BarberBoss.Application.UseCases.Billing.Update;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;

namespace Validator.Tests.UseCases.Billing.Update;

public class UpdateBillingUseCaseTest
{
    [Fact]
    public async Task Execute_BillingExists_UpdatesBillingAndCommits()
    {
        // Arrange
        var billingId = Guid.NewGuid();
        var billing = new BillingBuilder()
            .WithId(billingId)
            .Build();
        var request = new RequestBillingJsonBuilder()
            .WithServiceName("Barba")
            .WithAmount(35)
            .Build();
        var repository = new BillingUpdateOnlyRepositoryBuilder()
            .WithBilling(billing)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateBillingUseCase(repository, unitOfWork, TestMapper.Create());

        // Act
        await useCase.Execute(billingId, request);

        // Assert
        Assert.NotNull(repository.UpdatedBilling);
        Assert.Equal(request.ServiceName, repository.UpdatedBilling.ServiceName);
        Assert.Equal(request.Amount, repository.UpdatedBilling.Amount);
        Assert.Equal(BillingPaymentMethod.Pix, repository.UpdatedBilling.PaymentMethod);
        Assert.Equal(BillingStatus.Pago, repository.UpdatedBilling.Status);
        Assert.Equal(1, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_BillingDoesNotExist_ThrowsNotFoundExceptionAndDoesNotCommit()
    {
        // Arrange
        var request = new RequestBillingJsonBuilder().Build();
        var repository = new BillingUpdateOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateBillingUseCase(repository, unitOfWork, TestMapper.Create());

        // Act
        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Null(repository.UpdatedBilling);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }

    [Fact]
    public async Task Execute_InvalidRequest_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var billing = new BillingBuilder().Build();
        var request = new RequestBillingJsonBuilder()
            .WithInvalidBarberName()
            .Build();
        var repository = new BillingUpdateOnlyRepositoryBuilder()
            .WithBilling(billing)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var useCase = new UpdateBillingUseCase(repository, unitOfWork, TestMapper.Create());

        // Act
        var act = async () => await useCase.Execute(billing.Id, request);

        // Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Null(repository.UpdatedBilling);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
