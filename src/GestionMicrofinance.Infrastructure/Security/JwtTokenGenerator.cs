using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GestionMicrofinance.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(Personne personne)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, personne.Id.ToString()),
            new Claim(ClaimTypes.Email, personne.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_settings.ExpirationInDays),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
