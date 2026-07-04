using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionMicrofinance.Infrastructure.Persistence.Repositories;

public class PersonneRepository : IPersonneRepository
{
    private readonly ApplicationDbContext _context;

    public PersonneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Personne?> GetByIdAsync(int id) =>
        _context.Personnes.FirstOrDefaultAsync(p => p.Id == id);

    public Task<Personne?> GetByEmailAsync(string email) =>
        _context.Personnes.FirstOrDefaultAsync(p => p.Email == email);

    public Task<bool> EmailExistsAsync(string email) =>
        _context.Personnes.AnyAsync(p => p.Email == email);

    public async Task AddAsync(Personne personne) =>
        await _context.Personnes.AddAsync(personne);
}
