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
    public static string EXPENSES_CANNOT_FOR_THE_FUTURE => GetString(nameof(EXPENSES_CANNOT_FOR_THE_FUTURE));
    public static string EXPENSE_NOT_FOUND => GetString(nameof(EXPENSE_NOT_FOUND));
    public static string PAYMENT_TYPE_INVALID => GetString(nameof(PAYMENT_TYPE_INVALID));
    public static string TITLE_REQUIRED => GetString(nameof(TITLE_REQUIRED));
    public static string UNKNOWN_ERROR => GetString(nameof(UNKNOWN_ERROR));

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, Culture) ?? name;
    }
}
