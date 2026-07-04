using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Common.Interfaces;

public interface IPersonneRepository
{
    Task<Personne?> GetByIdAsync(int id);
    Task<Personne?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(Personne personne);
}
