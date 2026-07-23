using Microsoft.EntityFrameworkCore;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
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
            new GeoCoordinate(49.3043, -123.1443),
            405));

    context.Parks.Add(
        new Park(
            "Queen Elizabeth Park",
            new GeoCoordinate(49.2415, -123.1126),
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

if (!context.Trees.Any())
{
    var douglasFir = await context.Species.FirstAsync(
        s => s.CommonName == "Douglas Fir");

    var redMaple = await context.Species.FirstAsync(
        s => s.CommonName == "Red Maple");

    var stanleyPark = await context.Parks.FirstAsync(
        p => p.Name == "Stanley Park");

    var qePark = await context.Parks.FirstAsync(
        p => p.Name == "Queen Elizabeth Park");

    context.Trees.Add(
        new Tree(
            "TREE-001",
            douglasFir,
            stanleyPark,
            new GeoCoordinate(49.3043, -123.1443),
            TreeHealthStatus.Good,
            new DateOnly(2020, 1, 1),
            12,
            30));

    context.Trees.Add(
        new Tree(
            "TREE-002",
            redMaple,
            qePark,
            new GeoCoordinate(49.2415, -123.1126),
            TreeHealthStatus.Fair,
            new DateOnly(2018, 5, 15),
            8,
            20));

}
await context.SaveChangesAsync();
}
}