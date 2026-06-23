using AutoMapper;
using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billing.Update;

public class UpdateBillingUseCase : IUpdateBillingUseCase
{
    private readonly IBillingUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public UpdateBillingUseCase(
        IBillingUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unityOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(Guid id, RequestBillingJson request)
    {
        Validate(request);

        var billing = await _repository.GetById(id);

        if (billing is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        _mapper.Map(request, billing);

        _repository.Update(billing);

        await _unityOfWork.Commit();
    }

    private void Validate(RequestBillingJson request)
    {
        var validator = new BillingValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
