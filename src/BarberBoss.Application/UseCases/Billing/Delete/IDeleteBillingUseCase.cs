namespace BarberBoss.Application.UseCases.Billing.Delete;

public interface IDeleteBillingUseCase
{
    Task Execute(Guid id);
}
