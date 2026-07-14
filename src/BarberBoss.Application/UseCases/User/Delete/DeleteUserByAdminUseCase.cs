using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.User.Delete;

public class DeleteUserByAdminUseCase : IDeleteUserByAdminUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IBillingWriteOnlyRepository _billingWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserByAdminUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IBillingWriteOnlyRepository billingWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _billingWriteOnlyRepository = billingWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        await _billingWriteOnlyRepository.DeleteAllByUserId(id);

        var userWasDeleted = await _userWriteOnlyRepository.Delete(id);

        if (userWasDeleted == false)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
