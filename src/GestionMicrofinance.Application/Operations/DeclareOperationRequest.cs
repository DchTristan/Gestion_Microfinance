using System.ComponentModel.DataAnnotations;

namespace GestionMicrofinance.Application.Operations;

public class DeclareOperationRequest
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
    public decimal Montant { get; set; }

    public int CategorieId { get; set; }
}
