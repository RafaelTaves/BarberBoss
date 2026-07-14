using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberBoss.Infrastructure.Security.Tokens;

internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationInMinutes;
    private readonly string _singinKey;

    public JwtTokenGenerator(uint expirationTokenInMinutes, string signinKey)
    {
        _expirationInMinutes = expirationTokenInMinutes;
        _singinKey = signinKey;
    }
 
        
    public string Generate(User user)
    {

        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);

    }

    private SymmetricSecurityKey SecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_singinKey));
    }
}
