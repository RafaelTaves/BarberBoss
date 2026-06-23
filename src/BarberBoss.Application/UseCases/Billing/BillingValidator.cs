using BarberBoss.Communication.Requests.Billing;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billing;

public class BillingValidator : AbstractValidator<RequestBillingJson>
{
    public BillingValidator()
    {
        RuleFor(billing => billing.Date)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.BILLING_DATE_REQUIRED);

        RuleFor(billing => billing.BarberName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED)
            .Length(2, 80)
            .WithMessage(ResourceErrorMessages.BARBER_NAME_LENGTH);

        RuleFor(billing => billing.ClientName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_LENGTH);

        RuleFor(billing => billing.ServiceName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_REQUIRED)
            .Length(2, 120)
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_LENGTH);

        RuleFor(billing => billing.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);

        RuleFor(billing => billing.Amount)
            .Equal(0)
            .When(billing => IsStatusCanceled(billing.Status))
            .WithMessage(ResourceErrorMessages.CANCELED_BILLING_AMOUNT_MUST_BE_ZERO);

        RuleFor(billing => billing.PaymentMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.PAYMENT_METHOD_REQUIRED)
            .Must(BeValidEnum<BillingPaymentMethod>)
            .WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);

        RuleFor(billing => billing.Status)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.BILLING_STATUS_REQUIRED)
            .Must(BeValidEnum<BillingStatus>)
            .WithMessage(ResourceErrorMessages.BILLING_STATUS_INVALID);

        RuleFor(billing => billing.Notes)
            .MaximumLength(500)
            .WithMessage(ResourceErrorMessages.NOTES_MAX_LENGTH);
    }

    private static bool BeValidEnum<TEnum>(string value) where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase: true, out TEnum enumValue)
            && Enum.IsDefined(enumValue);
    }

    private static bool IsStatusCanceled(string status)
    {
        return Enum.TryParse(status, ignoreCase: true, out BillingStatus billingStatus)
            && billingStatus == BillingStatus.Cancelado;
    }
}
