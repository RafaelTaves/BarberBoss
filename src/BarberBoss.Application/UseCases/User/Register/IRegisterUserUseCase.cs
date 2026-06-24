using BarberBoss.Communication.Requests.User;
using BarberBoss.Communication.Responses.User;

namespace BarberBoss.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisteredUserJson request);
}
