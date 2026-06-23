using System.Globalization;
using System.Resources;

namespace BarberBoss.Exception;

public static class ResourceErrorMessages
{
    private static readonly ResourceManager ResourceManager = new(
        "BarberBoss.Exception.ExceptionsBase.ResourceErrorMessages",
        typeof(ResourceErrorMessages).Assembly);

    private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("pt-BR");

    public static string AMOUNT_MUST_BE_GREATER_THAN_ZERO => GetString(nameof(AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    public static string AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_TO_ZERO => GetString(nameof(AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_TO_ZERO));
    public static string BARBER_NAME_LENGTH => GetString(nameof(BARBER_NAME_LENGTH));
    public static string BARBER_NAME_REQUIRED => GetString(nameof(BARBER_NAME_REQUIRED));
    public static string BILLING_DATE_REQUIRED => GetString(nameof(BILLING_DATE_REQUIRED));
    public static string BILLING_STATUS_INVALID => GetString(nameof(BILLING_STATUS_INVALID));
    public static string BILLING_STATUS_REQUIRED => GetString(nameof(BILLING_STATUS_REQUIRED));
    public static string CANCELED_BILLING_AMOUNT_MUST_BE_ZERO => GetString(nameof(CANCELED_BILLING_AMOUNT_MUST_BE_ZERO));
    public static string CLIENT_NAME_LENGTH => GetString(nameof(CLIENT_NAME_LENGTH));
    public static string CLIENT_NAME_REQUIRED => GetString(nameof(CLIENT_NAME_REQUIRED));
    public static string EXPENSES_CANNOT_FOR_THE_FUTURE => GetString(nameof(EXPENSES_CANNOT_FOR_THE_FUTURE));
    public static string EXPENSE_NOT_FOUND => GetString(nameof(EXPENSE_NOT_FOUND));
    public static string NOTES_MAX_LENGTH => GetString(nameof(NOTES_MAX_LENGTH));
    public static string PAYMENT_METHOD_INVALID => GetString(nameof(PAYMENT_METHOD_INVALID));
    public static string PAYMENT_METHOD_REQUIRED => GetString(nameof(PAYMENT_METHOD_REQUIRED));
    public static string PAYMENT_TYPE_INVALID => GetString(nameof(PAYMENT_TYPE_INVALID));
    public static string SERVICE_NAME_LENGTH => GetString(nameof(SERVICE_NAME_LENGTH));
    public static string SERVICE_NAME_REQUIRED => GetString(nameof(SERVICE_NAME_REQUIRED));
    public static string TITLE_REQUIRED => GetString(nameof(TITLE_REQUIRED));
    public static string UNKNOWN_ERROR => GetString(nameof(UNKNOWN_ERROR));

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, Culture) ?? name;
    }
}
