using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionMicrofinance.Infrastructure.Persistence.Repositories;

public class CategorieRepository : ICategorieRepository
{
    private readonly ApplicationDbContext _context;

    public CategorieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Categorie?> GetByIdAsync(int id) =>
        _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<Categorie>> GetAllAsync() =>
        _context.Categories.OrderBy(c => c.Libelle).ToListAsync();

    public async Task AddAsync(Categorie categorie) =>
        await _context.Categories.AddAsync(categorie);
}
