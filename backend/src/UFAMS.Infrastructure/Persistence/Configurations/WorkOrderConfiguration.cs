using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderConfiguration :
    IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(
        EntityTypeBuilder<WorkOrder> builder)
    {
        builder.HasKey(
            w => w.Id);


        builder.Property(
                w => w.Description)
            .IsRequired()
            .HasMaxLength(500);


        builder.Property(
                w => w.Status)
            .HasConversion<int>()
            .IsRequired();


        builder.Property(
                w => w.CreatedDate)
            .IsRequired();


        builder.HasOne(
                w => w.Tree)
            .WithMany()
            .HasForeignKey(
                w => w.TreeId)
            .OnDelete(
                DeleteBehavior.Restrict);


        builder.HasOne(
                w => w.Inspection)
            .WithMany()
            .HasForeignKey(
                w => w.InspectionId)
            .OnDelete(
                DeleteBehavior.Restrict);


        builder.HasOne(
                w => w.AssignedEmployee)
            .WithMany()
            .HasForeignKey(
                w => w.AssignedEmployeeId)
            .OnDelete(
                DeleteBehavior.Restrict);
    }
}