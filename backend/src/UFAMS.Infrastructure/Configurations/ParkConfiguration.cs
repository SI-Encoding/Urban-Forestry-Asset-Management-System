using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Configurations;

public class ParkConfiguration :
    IEntityTypeConfiguration<Park>
{
    public void Configure(
        EntityTypeBuilder<Park> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.AreaInHectares)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();


        builder.OwnsOne(
            x => x.Location,
            location =>
            {
                location.Property(x => x.Latitude)
                    .HasColumnName("Latitude")
                    .IsRequired();

                location.Property(x => x.Longitude)
                    .HasColumnName("Longitude")
                    .IsRequired();
            });
    }
}