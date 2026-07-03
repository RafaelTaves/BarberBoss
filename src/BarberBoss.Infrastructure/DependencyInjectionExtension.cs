using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using BarberBoss.Infrastructure.Security.Tokens;
using BarberBoss.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddToken(services, configuration);
        AddDbContext(services, configuration);

        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration) 
    {
        var expirationInMinutes = uint.Parse(configuration["Settings:Jwt:ExpiresMinutes"]!);
        var signinKey = configuration["Settings:Jwt:SigninKey"];

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationInMinutes, signinKey!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBillingReadOnlyRepository, BillingRepository>();
        services.AddScoped<IBillingWriteOnlyRepository, BillingRepository>();
        services.AddScoped<IBillingUpdateOnlyRepository, BillingRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<BarberBossDbContext>(config => config.UseNpgsql(connectionString));
    }
}
