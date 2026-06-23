using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;

namespace BarberBoss.Application.UseCases.Billing.Update;

public interface IUpdateBillingUseCase
{
    Task Execute(Guid id, RequestBillingJson request);
}
