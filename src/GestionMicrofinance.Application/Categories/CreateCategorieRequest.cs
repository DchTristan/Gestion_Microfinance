using System.ComponentModel.DataAnnotations;
using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Application.Categories;

public class CreateCategorieRequest
{
    [Required]
    public string Libelle { get; set; } = string.Empty;

    public TypeCategorie Type { get; set; }
}
