using Microsoft.EntityFrameworkCore;
using UFAMS.Domain.Entities;

namespace UFAMS.Infrastructure.Persistence;

public class UFAMSDbContext : DbContext
{
    public UFAMSDbContext(
        DbContextOptions<UFAMSDbContext> options)
        : base(options)
    {
    }

    public DbSet<SyncAudit> SyncAudits => Set<SyncAudit>();

    public DbSet<SyncAuditEntry> SyncAuditEntries => Set<SyncAuditEntry>();
    
    public DbSet<Tree> Trees => Set<Tree>();

    public DbSet<Species> Species => Set<Species>();

    public DbSet<Park> Parks => Set<Park>();

    public DbSet<Inspection> Inspections => Set<Inspection>();

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UFAMSDbContext).Assembly);
        
        modelBuilder.Entity<Inspection>(builder =>
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ObservedHealth);

            builder.Property(i => i.Notes)
                .HasMaxLength(2000);

            builder.Property(i => i.Recommendation)
                .HasMaxLength(1000);

            builder.HasOne(i => i.Tree)
                .WithMany(t => t.Inspections)
                .HasForeignKey(i => i.TreeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SyncAudit>(entity =>
    {
        entity.HasKey(
            audit => audit.Id);

        entity.Property(
            audit => audit.Status)
            .HasConversion<string>()
            .IsRequired();

        entity.Property(
            audit => audit.StartedAt)
            .IsRequired();

        entity.HasMany(
            audit => audit.Entries)
            .WithOne(
                entry => entry.SyncAudit)
            .HasForeignKey(
                entry => entry.SyncAuditId)
            .OnDelete(
                DeleteBehavior.Cascade);
    });


    modelBuilder.Entity<SyncAuditEntry>(entity =>
    {
        entity.HasKey(
            entry => entry.Id);

        entity.Property(
            entry => entry.AssetTag)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(
            entry => entry.Action)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(
            entry => entry.Reason)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(
            entry => entry.CreatedAt)
            .IsRequired();
    });
    }
}