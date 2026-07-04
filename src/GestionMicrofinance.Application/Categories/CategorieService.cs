using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Domain.Entities;

namespace GestionMicrofinance.Application.Categories;

public class CategorieService : ICategorieService
{
    private readonly ICategorieRepository _categorieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategorieService(ICategorieRepository categorieRepository, IUnitOfWork unitOfWork)
    {
        _categorieRepository = categorieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategorieResponse> CreateAsync(CreateCategorieRequest request)
    {
        var categorie = new Categorie
        {
            Libelle = request.Libelle,
            Type = request.Type,
            CreatedDate = DateTime.UtcNow
        };

        await _categorieRepository.AddAsync(categorie);
        await _unitOfWork.SaveChangesAsync();

        return ToResponse(categorie);
    }

    public async Task<List<CategorieResponse>> GetAllAsync()
    {
        var categories = await _categorieRepository.GetAllAsync();
        return categories.Select(ToResponse).ToList();
    }

    private static CategorieResponse ToResponse(Categorie categorie) => new()
    {
        Id = categorie.Id,
        Libelle = categorie.Libelle,
        Type = categorie.Type,
        CreatedDate = categorie.CreatedDate
    };
}
