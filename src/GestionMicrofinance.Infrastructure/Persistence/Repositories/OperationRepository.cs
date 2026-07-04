using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionMicrofinance.Infrastructure.Persistence.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly ApplicationDbContext _context;

    public OperationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Operation operation) =>
        await _context.Operations.AddAsync(operation);

    public Task<List<Operation>> GetRecentByPersonneAsync(int personneId, DateTime depuis) =>
        _context.Operations
            .Include(o => o.Categorie)
            .Where(o => o.PersonneId == personneId && o.DateOperation >= depuis)
            .OrderByDescending(o => o.DateOperation)
            .ToListAsync();
}
