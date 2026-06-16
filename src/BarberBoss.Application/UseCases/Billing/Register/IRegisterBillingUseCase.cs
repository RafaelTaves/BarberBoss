using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;

namespace BarberBoss.Application.UseCases.Billing.Register;

public interface IRegisterBillingUseCase
{
    Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request);
}
