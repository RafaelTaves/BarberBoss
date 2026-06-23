using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billing.Delete;

public class DeleteBillingUseCase : IDeleteBillingUseCase
{
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillingUseCase(
        IBillingWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var billingWasDeleted = await _repository.Delete(id);

        if (billingWasDeleted == false)
        {
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
