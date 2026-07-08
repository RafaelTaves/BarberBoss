using BarberBoss.Communication.Requests.User;

namespace BarberBoss.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
