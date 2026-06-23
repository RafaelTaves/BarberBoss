using BarberBoss.Communication.Responses.Billing;

namespace BarberBoss.Application.UseCases.Billing.GetAll;

public interface IGetAllBillingsUseCase
{
    Task<ResponseBillingsJson> Execute();
}
