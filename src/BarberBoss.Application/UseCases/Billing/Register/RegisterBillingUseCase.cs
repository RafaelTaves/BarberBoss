using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;
using BillingEntity = BarberBoss.Domain.Entities.Billing;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using System.Net.Http.Headers;
using AutoMapper;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Billing.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase
{
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public RegisterBillingUseCase(
        IBillingWriteOnlyRepository repository,
        IUnitOfWork unityOfWork,
        IMapper mapper,
        ILoggedUser loggedUser
        )
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var billing = _mapper.Map<BillingEntity>(request);
        billing.UserId = loggedUser.Id;

        await _repository.Add(billing);

        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredBillingJson>(billing);
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
