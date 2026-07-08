using AutoMapper;
using BarberBoss.Communication.Requests.User;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public UpdateUserUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unityOfWork,
        IMapper mapper)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _loggedUser = loggedUser;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var user = await _loggedUser.Get();

        await Validate(request, user.Id);

        _mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;

        _userUpdateOnlyRepository.Update(user);

        await _unityOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, Guid loggedUserId)
    {
        var result = new UpdateUserValidator().Validate(request);

        var userWithEmail = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (userWithEmail is not null && userWithEmail.Id != loggedUserId)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
