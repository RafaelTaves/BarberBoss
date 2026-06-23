using BarberBoss.Communication.Responses.Billing;

namespace BarberBoss.Application.UseCases.Billing.GetById;

public interface IGetBillingByIdUseCase
{
    Task<ResponseBillingJson> Execute(Guid id);
}
