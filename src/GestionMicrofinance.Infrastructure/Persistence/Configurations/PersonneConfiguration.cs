using GestionMicrofinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionMicrofinance.Infrastructure.Persistence.Configurations;

public class PersonneConfiguration : IEntityTypeConfiguration<Personne>
{
    public void Configure(EntityTypeBuilder<Personne> builder)
    {
        builder.HasIndex(p => p.Email).IsUnique();

        builder.HasMany(p => p.Operations)
            .WithOne(o => o.Personne)
            .HasForeignKey(o => o.PersonneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
