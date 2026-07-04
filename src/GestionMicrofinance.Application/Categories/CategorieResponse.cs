using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Application.Categories;

public class CategorieResponse
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public TypeCategorie Type { get; set; }
    public DateTime CreatedDate { get; set; }
}
