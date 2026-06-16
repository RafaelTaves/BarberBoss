using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Responses.Billing;
using System.Net.Http.Headers;

namespace BarberBoss.Application.UseCases.Billing.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase
{

    public async Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request)
    {
        Validate(request);

        var entity = _mapper.Map<Billing>(request);

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
