namespace GestionMicrofinance.Application.Categories;

public interface ICategorieService
{
    Task<CategorieResponse> CreateAsync(CreateCategorieRequest request);
    Task<List<CategorieResponse>> GetAllAsync();
}
