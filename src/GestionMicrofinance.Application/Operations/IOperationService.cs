using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Application.Operations;

public interface IOperationService
{
    Task<OperationResponse> DeclareAsync(int personneId, DeclareOperationRequest request, TypeCategorie type);
    Task<List<OperationResponse>> GetRecentAsync(int personneId, TypeCategorie type, int moisAvant = 3);
}
