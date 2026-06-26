using BarberBoss.Communication.Requests.Login;
using BarberBoss.Communication.Responses.User;

namespace BarberBoss.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
