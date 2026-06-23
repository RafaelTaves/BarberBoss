using AutoMapper;
using BarberBoss.Communication.Responses.Billing;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billing.GetAll;

public class GetAllBillingsJson : IGetAllBillingsUseCase
{
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    public GetAllBillingsJson(
        IBillingReadOnlyRepository repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseBillingsJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseBillingsJson
        {
            Billings = _mapper.Map<List<ResponseShortBillingJson>>(result)
        }; 

    }
}
