using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Domain.Entities;

public class Categorie
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public TypeCategorie Type { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
