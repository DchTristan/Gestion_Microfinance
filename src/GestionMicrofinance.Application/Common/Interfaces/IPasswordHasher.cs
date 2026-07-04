namespace GestionMicrofinance.Application.Common.Interfaces;

public interface IPasswordHasher
{
    void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool Verify(string password, byte[] passwordHash, byte[] passwordSalt);
}
