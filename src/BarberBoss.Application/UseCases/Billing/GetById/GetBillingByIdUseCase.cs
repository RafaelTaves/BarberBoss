using AutoMapper;
using BarberBoss.Communication.Responses.Billing;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billing.GetById;

public class GetBillingByIdUseCase : IGetBillingByIdUseCase
{
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetBillingByIdUseCase(
        IBillingReadOnlyRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseBillingJson> Execute(Guid id)
    {
        var billing = await _repository.GetById(id);

        if (billing is null)
        {
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
        }

        return _mapper.Map<ResponseBillingJson>(billing);
    }
}
