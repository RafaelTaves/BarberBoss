using BarberBoss.Application.UseCases.Billing.GetAll;
using Validator.Tests.Support.AutoMapper;
using Validator.Tests.Support.Builders;
using Validator.Tests.Support.Repositories;

namespace Validator.Tests.UseCases.Billing.GetAll;

public class GetAllBillingsUseCaseTest
{
    [Fact]
    public async Task Execute_BillingsExist_ReturnsBillings()
    {
        // Arrange
        var firstBilling = new BillingBuilder().Build();
        var secondBilling = new BillingBuilder().Build();
        var repository = new BillingReadOnlyRepositoryBuilder()
            .WithBillings([firstBilling, secondBilling])
            .Build();
        var useCase = new GetAllBillingsJson(repository, TestMapper.Create());

        // Act
        var response = await useCase.Execute();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Billings.Count);
        Assert.Contains(response.Billings, billing => billing.Id == firstBilling.Id);
        Assert.Contains(response.Billings, billing => billing.Id == secondBilling.Id);
    }

    [Fact]
    public async Task Execute_BillingsDoNotExist_ReturnsEmptyList()
    {
        // Arrange
        var repository = new BillingReadOnlyRepositoryBuilder().Build();
        var useCase = new GetAllBillingsJson(repository, TestMapper.Create());

        // Act
        var response = await useCase.Execute();

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.Billings);
    }
}
