using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionMicrofinance.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Personne> Personnes => Set<Personne>();
    public DbSet<Categorie> Categories => Set<Categorie>();
    public DbSet<Operation> Operations => Set<Operation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
