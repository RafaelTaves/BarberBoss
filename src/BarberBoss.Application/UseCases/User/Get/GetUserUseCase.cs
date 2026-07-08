using AutoMapper;
using BarberBoss.Communication.Responses.User;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.User.Get;

public class GetUserUseCase : IGetUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserUseCase(
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserJson> Execute()
    {
        var user = await _loggedUser.Get();

        return _mapper.Map<ResponseUserJson>(user);
    }
}
