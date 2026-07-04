using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Personne personne);
}
