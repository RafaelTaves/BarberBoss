using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.User.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(
        IUserWriteOnlyRepository repository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var userWasDeleted = await _repository.Delete(loggedUser.Id);

        if (userWasDeleted == false)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
