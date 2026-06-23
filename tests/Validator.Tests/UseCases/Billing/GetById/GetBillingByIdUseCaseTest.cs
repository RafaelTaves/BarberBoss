using BarberBoss.Application.UseCases.Billing.GetById;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception.ExceptionsBase;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;

namespace Validator.Tests.UseCases.Billing.GetById;

public class GetBillingByIdUseCaseTest
{
    [Fact]
    public async Task Execute_BillingExists_ReturnsBilling()
    {
        // Arrange
        var billingId = Guid.NewGuid();
        var billing = new BillingBuilder()
            .WithId(billingId)
            .Build();
        var repository = new BillingReadOnlyRepositoryBuilder()
            .WithBilling(billing)
            .Build();
        var useCase = new GetBillingByIdUseCase(repository, TestMapper.Create());

        // Act
        var response = await useCase.Execute(billingId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(billing.Id, response.Id);
        Assert.Equal(billing.Date, response.Date);
        Assert.Equal(billing.BarberName, response.BarberName);
        Assert.Equal(billing.ClientName, response.ClientName);
        Assert.Equal(billing.ServiceName, response.ServiceName);
        Assert.Equal(billing.Amount, response.Amount);
        Assert.Equal(BillingPaymentMethod.Pix.ToString(), response.PaymentMethod);
        Assert.Equal(BillingStatus.Pago.ToString(), response.Status);
        Assert.Equal(billing.Notes, response.Notes);
    }

    [Fact]
    public async Task Execute_BillingDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new BillingReadOnlyRepositoryBuilder().Build();
        var useCase = new GetBillingByIdUseCase(repository, TestMapper.Create());

        // Act
        var act = async () => await useCase.Execute(Guid.NewGuid());

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
}
