using AutoMapper;
using BarberBoss.Application.AutoMapper;

namespace Validator.Tests.Support.AutoMapper;

public static class TestMapper
{
    public static IMapper Create()
    {
        var configuration = new MapperConfiguration(config => config.AddProfile<AutoMapping>());

        return configuration.CreateMapper();
    }
}
