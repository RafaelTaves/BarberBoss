using AutoMapper;
using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Communication.Requests.User;
using BarberBoss.Communication.Responses.Billing;
using BarberBoss.Communication.Responses.User;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;

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
        CreateMap<RequestBillingJson, Billing>()
            .ForMember(billing => billing.PaymentMethod, config => config.MapFrom(request =>
                Enum.Parse<BillingPaymentMethod>(request.PaymentMethod, true)))
            .ForMember(billing => billing.Status, config => config.MapFrom(request =>
                Enum.Parse<BillingStatus>(request.Status, true)));
        CreateMap<RequestRegisteredUserJson, User>()
            .ForMember(user => user.PasswordHash, config => config.Ignore());
    }

    private void EntityToResponse()
    {
        CreateMap<Billing, ResponseBillingJson>();
        CreateMap<Billing, ResponseRegisteredBillingJson>();
        CreateMap<Billing, ResponseShortBillingJson>();
        CreateMap<User, ResponseUserJson>();
        CreateMap<User, ResponseRegisteredUserJson>()
            .ForMember(response => response.Token, config => config.MapFrom((_, _, _, context) =>
                context.TryGetItems(out var items) && items.TryGetValue("Token", out var token)
                    ? token?.ToString()
                    : string.Empty));
    }
}
