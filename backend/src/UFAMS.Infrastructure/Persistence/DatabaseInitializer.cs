using Microsoft.EntityFrameworkCore;
using UFAMS.Domain.Entities;
using UFAMS.Domain.ValueObjects;
using UFAMS.Infrastructure.Persistence;

namespace UFAMS.Infrastructure.Persistence;
public static class DatabaseInitializer
{
    public static async Task SeedAsync(UFAMSDbContext context)
    {
        if (!context.Species.Any())
        {
            context.Species.Add(
                new Species(
                    "Douglas Fir",
                    "Pseudotsuga menziesii",
                    true));

            context.Species.Add(
                new Species(
                    "Red Maple",
                    "Acer rubrum",
                    false));
        }

        if (!context.Parks.Any())
        {
            context.Parks.Add(
                new Park(
                    "Stanley Park",
                    new GeoCoordinate(
                        49.3043,
                        -123.1443),
                    405));

            context.Parks.Add(
                new Park(
                    "Queen Elizabeth Park",
                    new GeoCoordinate(
                        49.2415,
                        -123.1126),
                    52));
        }

        if (!context.Employees.Any())
        {
            context.Employees.Add(
                new Employee(
                    "John Smith",
                    "Arborist"));

            context.Employees.Add(
                new Employee(
                    "Sarah Johnson",
                    "Tree Technician"));
        }

        await context.SaveChangesAsync();
    }
}