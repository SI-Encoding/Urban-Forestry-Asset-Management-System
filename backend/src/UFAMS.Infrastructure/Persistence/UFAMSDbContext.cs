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

    public DbSet<Tree> Trees => Set<Tree>();

    public DbSet<Species> Species => Set<Species>();

    public DbSet<Park> Parks => Set<Park>();

    public DbSet<Inspection> Inspections => Set<Inspection>();

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
    }
}