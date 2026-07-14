namespace BarberBoss.Application.UseCases.User.Delete;

public interface IDeleteUserByAdminUseCase
{
    Task Execute(Guid id);
}
