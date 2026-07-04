namespace GestionMicrofinance.Domain.Entities;

public class Operation
{
    public int Id { get; set; }
    public DateTime DateOperation { get; set; }
    public decimal Montant { get; set; }

    public int PersonneId { get; set; }
    public Personne? Personne { get; set; }

    public int CategorieId { get; set; }
    public Categorie? Categorie { get; set; }
}
