using BarberBoss.Domain.Security.Cryptography;

namespace Validator.Tests.Support.Security;

public class PasswordEncripterDouble : IPasswordEncripter
{
    private readonly bool _passwordMatches;
    private readonly string _encryptedPassword;

    public PasswordEncripterDouble(bool passwordMatches, string encryptedPassword)
    {
        _passwordMatches = passwordMatches;
        _encryptedPassword = encryptedPassword;
    }

    public string? PasswordToEncrypt { get; private set; }
    public string? PasswordToVerify { get; private set; }
    public string? PasswordHashToVerify { get; private set; }

    public string Encrypt(string password)
    {
        PasswordToEncrypt = password;

        return _encryptedPassword;
    }

    public bool Verify(string password, string passwordHash)
    {
        PasswordToVerify = password;
        PasswordHashToVerify = passwordHash;

        return _passwordMatches;
    }
}
