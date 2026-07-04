using GestionMicrofinance.Domain.Enums;

namespace GestionMicrofinance.Domain.Entities;

public class Personne
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public Role Role { get; set; } = Role.Utilisateur;
    public DateTime DateCreation { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
