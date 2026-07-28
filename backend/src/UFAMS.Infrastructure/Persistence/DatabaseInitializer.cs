using Microsoft.EntityFrameworkCore;
using UFAMS.Infrastructure.Persistence.SeedData;

namespace UFAMS.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(UFAMSDbContext context)
    {
        if (!context.Species.Any())
        {
            context.Species.AddRange(
                SpeciesSeed.Create());
        }

        if (!context.Parks.Any())
        {
            context.Parks.AddRange(
                ParkSeed.Create());
        }

        if (!context.Employees.Any())
        {
            context.Employees.AddRange(
                EmployeeSeed.Create());
        }

        await context.SaveChangesAsync();

        if (!context.Trees.Any())
        {
            var trees = TreeSeed.Create(
                await context.Species.ToListAsync(),
                await context.Parks.ToListAsync());

            context.Trees.AddRange(trees);

            await context.SaveChangesAsync();
        }

        if (!context.Inspections.Any())
        {
            var trees = await context.Trees.ToListAsync();

            var inspections = InspectionSeed.Create(trees);

            context.Inspections.AddRange(inspections);

            await context.SaveChangesAsync();
        }

        if (!context.WorkOrders.Any())
        {
            var trees = await context.Trees.ToListAsync();

            var inspections = await context.Inspections.ToListAsync();

            var employees = await context.Employees.ToListAsync();

            var workOrders = WorkOrderSeed.Create(
                trees,
                inspections,
                employees);

            context.WorkOrders.AddRange(workOrders);

            await context.SaveChangesAsync();
        }
    }
}