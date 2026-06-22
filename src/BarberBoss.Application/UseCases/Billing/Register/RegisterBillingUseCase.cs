using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;
using BillingEntity = BarberBoss.Domain.Entities.Billing;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using System.Net.Http.Headers;
using AutoMapper;

namespace BarberBoss.Application.UseCases.Billing.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase
{
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    public RegisterBillingUseCase(
        IBillingWriteOnlyRepository repository,
        IUnitOfWork unityOfWork,
        IMapper mapper
        )
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request)
    {
        Validate(request);

        var entity = _mapper.Map<BillingEntity>(request);

        await _repository.Add(entity);

        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredBillingJson>(entity);
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
