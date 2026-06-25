using AutoMapper;
using BarberBoss.Communication.Requests.User;
using BarberBoss.Communication.Responses.User;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;
using UserEntity = BarberBoss.Domain.Entities.User;

namespace BarberBoss.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyrepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository userWriteOnlyrepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unityOfWork,
        IMapper mapper,
        IPasswordEncripter passwordEncripter)
    {
        _userWriteOnlyrepository = userWriteOnlyrepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisteredUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<UserEntity>(request);
        user.PasswordHash = _passwordEncripter.Encrypt(request.Password);
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _userWriteOnlyrepository.Add(user);

        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredUserJson>(user);
    }

    private async Task Validate(RequestRegisteredUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
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
