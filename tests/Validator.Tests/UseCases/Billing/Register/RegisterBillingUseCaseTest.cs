using BarberBoss.Application.UseCases.Billing.Register;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;
using Validator.Tests.Support.Services;

namespace Validator.Tests.UseCases.Billing.Register;

public class RegisterBillingUseCaseTest
{
    [Fact]
    public async Task Execute_ValidRequest_AddsBillingAndCommits()
    {
        // Arrange
        var request = new RequestBillingJsonBuilder().Build();
        var repository = new BillingWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var loggedUser = new LoggedUserDouble(new UserBuilder().Build());
        var useCase = new RegisterBillingUseCase(repository, unitOfWork, TestMapper.Create(), loggedUser);

        // Act
        var response = await useCase.Execute(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(repository.AddedBilling);
        Assert.Equal(request.ServiceName, repository.AddedBilling.ServiceName);
        Assert.Equal(request.Amount, repository.AddedBilling.Amount);
        Assert.Equal(BillingPaymentMethod.Pix, repository.AddedBilling.PaymentMethod);
        Assert.Equal(BillingStatus.Pago, repository.AddedBilling.Status);
        Assert.Equal(1, unitOfWork.CommitsCount);
        Assert.Equal(repository.AddedBilling.Id, response.Id);
        Assert.Equal(request.ServiceName, response.ServiceName);
        Assert.Equal(request.Amount, response.Amount);
    }

    [Fact]
    public async Task Execute_InvalidRequest_ThrowsValidationExceptionAndDoesNotCommit()
    {
        // Arrange
        var request = new RequestBillingJsonBuilder()
            .WithInvalidBarberName()
            .Build();
        var repository = new BillingWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var loggedUser = new LoggedUserDouble(new UserBuilder().Build());
        var useCase = new RegisterBillingUseCase(repository, unitOfWork, TestMapper.Create(), loggedUser);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Null(repository.AddedBilling);
        Assert.Equal(0, unitOfWork.CommitsCount);
    }
}
