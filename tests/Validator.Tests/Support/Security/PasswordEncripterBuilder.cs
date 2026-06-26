namespace Validator.Tests.Support.Security;

public class PasswordEncripterBuilder
{
    private bool _passwordMatches = true;
    private string _encryptedPassword = "encrypted-password";

    public PasswordEncripterBuilder WithPasswordMatches(bool passwordMatches)
    {
        _passwordMatches = passwordMatches;
        return this;
    }

    public PasswordEncripterBuilder WithEncryptedPassword(string encryptedPassword)
    {
        _encryptedPassword = encryptedPassword;
        return this;
    }

    public PasswordEncripterDouble Build()
    {
        return new PasswordEncripterDouble(_passwordMatches, _encryptedPassword);
    }
}
