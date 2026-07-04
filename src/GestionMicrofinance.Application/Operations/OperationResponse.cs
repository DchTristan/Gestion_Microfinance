namespace GestionMicrofinance.Application.Operations;

public class OperationResponse
{
    public int Id { get; set; }
    public DateTime DateOperation { get; set; }
    public decimal Montant { get; set; }
    public int CategorieId { get; set; }
    public string CategorieLibelle { get; set; } = string.Empty;
}
