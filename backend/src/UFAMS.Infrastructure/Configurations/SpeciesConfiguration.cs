using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Configurations;

public class SpeciesConfiguration :
    IEntityTypeConfiguration<Species>
{
    public void Configure(
        EntityTypeBuilder<Species> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CommonName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ScientificName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}