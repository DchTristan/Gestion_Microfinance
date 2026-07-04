namespace GestionMicrofinance.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
