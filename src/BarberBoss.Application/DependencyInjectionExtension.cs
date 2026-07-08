using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Billing.Delete;
using BarberBoss.Application.UseCases.Billing.GetAll;
using BarberBoss.Application.UseCases.Billing.GetById;
using BarberBoss.Application.UseCases.Billing.Register;
using BarberBoss.Application.UseCases.Billing.Update;
using BarberBoss.Application.UseCases.Login;
using BarberBoss.Application.UseCases.User.Get;
using BarberBoss.Application.UseCases.User.Register;
using BarberBoss.Application.UseCases.User.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterBillingUseCase, RegisterBillingUseCase>();
        services.AddScoped<IUpdateBillingUseCase, UpdateBillingUseCase>();
        services.AddScoped<IGetAllBillingsUseCase, GetAllBillingsJson>();
        services.AddScoped<IGetBillingByIdUseCase, GetBillingByIdUseCase>();
        services.AddScoped<IDeleteBillingUseCase, DeleteBillingUseCase>();
        services.AddScoped<IGetUserUseCase, GetUserUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>(); 
    }
}
