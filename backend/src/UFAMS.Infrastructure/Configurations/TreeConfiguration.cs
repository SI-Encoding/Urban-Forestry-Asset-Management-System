using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Configurations;

public class TreeConfiguration :
    IEntityTypeConfiguration<Tree>
{
    public void Configure(
        EntityTypeBuilder<Tree> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.AssetTag)
            .HasMaxLength(50)
            .IsRequired();


        builder.HasIndex(x => x.AssetTag)
            .IsUnique();


        builder.Property(t => t.ArcGisFeatureId)
            .HasMaxLength(100);

        builder.HasIndex(t => t.ArcGisFeatureId);

        builder.HasOne(x => x.Species)
            .WithMany()
            .HasForeignKey("SpeciesId")
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(x => x.Park)
            .WithMany()
            .HasForeignKey("ParkId")
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property(x => x.PlantingDate)
            .IsRequired();


        builder.Property(x => x.HeightInMeters)
            .IsRequired();


        builder.Property(x => x.DiameterInCentimeters)
            .IsRequired();


        builder.Property(x => x.HealthStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
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