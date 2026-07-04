using GestionMicrofinance.Application.Common.Exceptions;
using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;
using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Application.Operations;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _operationRepository;
    private readonly ICategorieRepository _categorieRepository;
    private readonly IPersonneRepository _personneRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OperationService(
        IOperationRepository operationRepository,
        ICategorieRepository categorieRepository,
        IPersonneRepository personneRepository,
        IUnitOfWork unitOfWork)
    {
        _operationRepository = operationRepository;
        _categorieRepository = categorieRepository;
        _personneRepository = personneRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResponse> DeclareAsync(int personneId, DeclareOperationRequest request, TypeCategorie type)
    {
        var personne = await _personneRepository.GetByIdAsync(personneId);
        if (personne is null)
        {
            throw new NotFoundException("Utilisateur non trouvé.");
        }

        var categorie = await _categorieRepository.GetByIdAsync(request.CategorieId);
        if (categorie is null)
        {
            throw new NotFoundException("Catégorie non trouvée.");
        }

        if (categorie.Type != type)
        {
            throw new ValidationAppException(
                $"La catégorie '{categorie.Libelle}' n'est pas de type '{type}'.");
        }

        var operation = new Operation
        {
            Montant = request.Montant,
            PersonneId = personne.Id,
            CategorieId = categorie.Id,
            DateOperation = DateTime.UtcNow
        };

        await _operationRepository.AddAsync(operation);
        await _unitOfWork.SaveChangesAsync();

        return new OperationResponse
        {
            Id = operation.Id,
            DateOperation = operation.DateOperation,
            Montant = operation.Montant,
            CategorieId = categorie.Id,
            CategorieLibelle = categorie.Libelle
        };
    }

    public async Task<List<OperationResponse>> GetRecentAsync(int personneId, TypeCategorie type, int moisAvant = 3)
    {
        var depuis = DateTime.UtcNow.AddMonths(-moisAvant);
        var operations = await _operationRepository.GetRecentByPersonneAsync(personneId, depuis);

        return operations
            .Where(o => o.Categorie is not null && o.Categorie.Type == type)
            .Select(o => new OperationResponse
            {
                Id = o.Id,
                DateOperation = o.DateOperation,
                Montant = o.Montant,
                CategorieId = o.CategorieId,
                CategorieLibelle = o.Categorie?.Libelle ?? string.Empty
            }).ToList();
    }
}
