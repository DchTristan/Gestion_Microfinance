using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Common.Interfaces;

public interface IOperationRepository
{
    Task AddAsync(Operation operation);
    Task<List<Operation>> GetRecentByPersonneAsync(int personneId, DateTime depuis);
}
