using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionMicrofinance.Infrastructure.Persistence.Configurations;

public class CategorieConfiguration : IEntityTypeConfiguration<Categorie>
{
    public void Configure(EntityTypeBuilder<Categorie> builder)
    {
        builder.HasMany(c => c.Operations)
            .WithOne(o => o.Categorie)
            .HasForeignKey(o => o.CategorieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
