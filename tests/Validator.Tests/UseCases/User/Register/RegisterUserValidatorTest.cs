using BarberBoss.Application.UseCases.User.Register;
using BarberBoss.Exception;
using Validator.Tests.Support.Builders;

namespace Validator.Tests.UseCases.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Validate_ValidRequest_ReturnsValid()
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder().Build();
        var validator = new RegisterUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "USER_NAME_REQUIRED")]
    [InlineData("A", "USER_NAME_LENGTH")]
    public void Validate_InvalidName_ReturnsExpectedMessage(string name, string expectedResourceKey)
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder()
            .WithName(name)
            .Build();
        var validator = new RegisterUserValidator();
        var expectedMessage = GetExpectedMessage(expectedResourceKey);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == expectedMessage);
    }

    [Theory]
    [InlineData("", "EMAIL_REQUIRED")]
    [InlineData("invalid-email", "EMAIL_INVALID")]
    public void Validate_InvalidEmail_ReturnsExpectedMessage(string email, string expectedResourceKey)
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder()
            .WithEmail(email)
            .Build();
        var validator = new RegisterUserValidator();
        var expectedMessage = GetExpectedMessage(expectedResourceKey);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == expectedMessage);
    }

    [Theory]
    [InlineData("", "PASSWORD_REQUIRED")]
    [InlineData("A1!", "PASSWORD_MIN_LENGTH")]
    [InlineData("abc123!", "PASSWORD_UPPERCASE_LETTER")]
    [InlineData("ABC123!", "PASSWORD_LOWERCASE_LETTER")]
    [InlineData("Abcdef!", "PASSWORD_NUMBER")]
    [InlineData("Abc123", "PASSWORD_SPECIAL_CHARACTER")]
    public void Validate_InvalidPassword_ReturnsExpectedMessage(string password, string expectedResourceKey)
    {
        // Arrange
        var request = new RequestRegisteredUserJsonBuilder()
            .WithPassword(password)
            .Build();
        var validator = new RegisterUserValidator();
        var expectedMessage = GetExpectedMessage(expectedResourceKey);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == expectedMessage);
        Assert.DoesNotContain(result.Errors, error => error.ErrorMessage == "ErrorMessage");
    }

    private static string GetExpectedMessage(string resourceKey)
    {
        return resourceKey switch
        {
            "USER_NAME_REQUIRED" => ResourceErrorMessages.USER_NAME_REQUIRED,
            "USER_NAME_LENGTH" => ResourceErrorMessages.USER_NAME_LENGTH,
            "EMAIL_REQUIRED" => ResourceErrorMessages.EMAIL_REQUIRED,
            "EMAIL_INVALID" => ResourceErrorMessages.EMAIL_INVALID,
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
