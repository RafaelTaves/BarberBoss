using BarberBoss.Communication.Responses.User;

namespace BarberBoss.Application.UseCases.User.Get;

public interface IGetUserUseCase
{
    Task<ResponseUserJson> Execute();
}
