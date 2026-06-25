using BarberBoss.Application.UseCases.User.Register;
using BarberBoss.Communication.Requests.User;
using BarberBoss.Exception;

namespace Validator.Tests.UseCases.User.Register;

public class RegisterUserValidatorTest
{
    [Theory]
    [InlineData("", "PASSWORD_REQUIRED")]
    [InlineData("A1!", "PASSWORD_MIN_LENGTH")]
    [InlineData("abc123!", "PASSWORD_UPPERCASE_LETTER")]
    [InlineData("ABC123!", "PASSWORD_LOWERCASE_LETTER")]
    [InlineData("Abcdef!", "PASSWORD_NUMBER")]
    [InlineData("Abc123", "PASSWORD_SPECIAL_CHARACTER")]
    public void Validate_InvalidPassword_ReturnsExpectedMessage(string password, string expectedResourceKey)
    {
        var request = new RequestRegisteredUserJson
        {
            Name = "Rafael",
            Email = "rafael@email.com",
            Password = password
        };
        var validator = new RegisterUserValidator();
        var expectedMessage = GetExpectedMessage(expectedResourceKey);

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == expectedMessage);
        Assert.DoesNotContain(result.Errors, error => error.ErrorMessage == "ErrorMessage");
    }

    private static string GetExpectedMessage(string resourceKey)
    {
        return resourceKey switch
        {
            "PASSWORD_REQUIRED" => ResourceErrorMessages.PASSWORD_REQUIRED,
            "PASSWORD_MIN_LENGTH" => ResourceErrorMessages.PASSWORD_MIN_LENGTH,
            "PASSWORD_UPPERCASE_LETTER" => ResourceErrorMessages.PASSWORD_UPPERCASE_LETTER,
            "PASSWORD_LOWERCASE_LETTER" => ResourceErrorMessages.PASSWORD_LOWERCASE_LETTER,
            "PASSWORD_NUMBER" => ResourceErrorMessages.PASSWORD_NUMBER,
            "PASSWORD_SPECIAL_CHARACTER" => ResourceErrorMessages.PASSWORD_SPECIAL_CHARACTER,
            _ => throw new ArgumentException("Invalid resource key.", nameof(resourceKey))
        };
    }
}
