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


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UFAMSDbContext).Assembly);
    }
}