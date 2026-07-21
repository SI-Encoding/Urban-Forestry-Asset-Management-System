using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence.Configurations;

public sealed class EmployeeConfiguration :
    IEntityTypeConfiguration<Employee>
{
    public void Configure(
        EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(
            e => e.Id);


        builder.Property(
                e => e.Name)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(
                e => e.Role)
            .IsRequired()
            .HasMaxLength(100);
    }
}