using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class InspectionSeed
{
    private static readonly Random Random = new(42);

    public static IReadOnlyList<Inspection> Create(
        IReadOnlyList<Tree> trees)
    {
        var inspections = new List<Inspection>();

        foreach (var tree in trees)
        {
            if (!ShouldInspect(tree.HealthStatus))
            {
                continue;
            }

            var inspectionDate = RandomInspectionDate();

            var observedHealth =
                DetermineObservedHealth(tree.HealthStatus);

            inspections.Add(
                new Inspection(
                    tree.Id,
                    inspectionDate,
                    observedHealth,
                    NotesFor(observedHealth),
                    RecommendationFor(observedHealth),
                    NextInspectionDate(
                        inspectionDate,
                        observedHealth)));
        }

        return inspections;
    }

    private static bool ShouldInspect(
        TreeHealthStatus health)
    {
        var roll = Random.NextDouble();

        return health switch
        {
            TreeHealthStatus.Excellent => roll < 0.30,
            TreeHealthStatus.Good => roll < 0.50,
            TreeHealthStatus.Fair => roll < 0.80,
            TreeHealthStatus.Poor => true,
            TreeHealthStatus.Dead => true,
            _ => false
        };
    }

    private static DateOnly RandomInspectionDate()
    {
        return new DateOnly(
            Random.Next(2022, 2026),
            Random.Next(1, 13),
            Random.Next(1, 28));
    }

    private static TreeHealthStatus DetermineObservedHealth(
        TreeHealthStatus current)
    {
        // 80% chance the inspection agrees with the current record.
        if (Random.NextDouble() < 0.80)
        {
            return current;
        }

        return current switch
        {
            TreeHealthStatus.Excellent => TreeHealthStatus.Good,

            TreeHealthStatus.Good =>
                Random.Next(2) == 0
                    ? TreeHealthStatus.Excellent
                    : TreeHealthStatus.Fair,

            TreeHealthStatus.Fair =>
                Random.Next(2) == 0
                    ? TreeHealthStatus.Good
                    : TreeHealthStatus.Poor,

            TreeHealthStatus.Poor =>
                Random.Next(2) == 0
                    ? TreeHealthStatus.Fair
                    : TreeHealthStatus.Dead,

            TreeHealthStatus.Dead =>
                TreeHealthStatus.Dead,

            _ => current
        };
    }

    private static DateOnly? NextInspectionDate(
        DateOnly inspectionDate,
        TreeHealthStatus health)
    {
        return health switch
        {
            TreeHealthStatus.Excellent =>
                inspectionDate.AddMonths(36),

            TreeHealthStatus.Good =>
                inspectionDate.AddMonths(24),

            TreeHealthStatus.Fair =>
                inspectionDate.AddMonths(12),

            TreeHealthStatus.Poor =>
                inspectionDate.AddMonths(6),

            TreeHealthStatus.Dead =>
                null,

            _ =>
                inspectionDate.AddMonths(12)
        };
    }

    private static string NotesFor(
        TreeHealthStatus health)
    {
        return health switch
        {
            TreeHealthStatus.Excellent =>
                "Excellent condition. No defects observed.",

            TreeHealthStatus.Good =>
                "Healthy tree with minor seasonal growth.",

            TreeHealthStatus.Fair =>
                "Minor structural defects observed. Routine pruning recommended.",

            TreeHealthStatus.Poor =>
                "Significant decline observed. Maintenance required.",

            TreeHealthStatus.Dead =>
                "Tree is dead and presents a potential hazard.",

            _ =>
                "Inspection completed."
        };
    }

    private static string RecommendationFor(
        TreeHealthStatus health)
    {
        return health switch
        {
            TreeHealthStatus.Excellent =>
                "Continue routine monitoring.",

            TreeHealthStatus.Good =>
                "Reinspect during the next maintenance cycle.",

            TreeHealthStatus.Fair =>
                "Schedule pruning within the next year.",

            TreeHealthStatus.Poor =>
                "Create corrective maintenance work order.",

            TreeHealthStatus.Dead =>
                "Schedule tree removal immediately.",

            _ =>
                "Monitor."
        };
    }
}