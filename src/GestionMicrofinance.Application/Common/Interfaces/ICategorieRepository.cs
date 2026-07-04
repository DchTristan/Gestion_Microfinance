using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Common.Interfaces;

public interface ICategorieRepository
{
    Task<Categorie?> GetByIdAsync(int id);
    Task<List<Categorie>> GetAllAsync();
    Task AddAsync(Categorie categorie);
}
