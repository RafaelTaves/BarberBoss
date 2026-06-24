using AutoMapper;
using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Requests.User;
using BarberBoss.Communication.Responses.Billing;
using BarberBoss.Communication.Responses.User;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestBillingJson, Billing>();
        CreateMap<RequestRegisteredUserJson, User>()
            .ForMember(user => user.PasswordHash, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Billing, ResponseBillingJson>();
        CreateMap<Billing, ResponseRegisteredBillingJson>();
        CreateMap<Billing, ResponseShortBillingJson>();
        CreateMap<User, ResponseRegisteredUserJson>()
            .ForMember(response => response.Token, config => config.MapFrom(_ => string.Empty));
    }
}
