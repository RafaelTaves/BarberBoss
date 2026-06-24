using BarberBoss.Communication.Requests.User;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisteredUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.USER_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.USER_NAME_LENGTH);

        RuleFor(user => user.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisteredUserJson>());
    }
}
