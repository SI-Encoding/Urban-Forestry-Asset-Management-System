using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;

namespace UFAMS.Infrastructure.Persistence.SeedData;

public static class WorkOrderSeed
{
    private static readonly Random Random = new(42);

    public static IReadOnlyList<WorkOrder> Create(
        IReadOnlyList<Tree> trees,
        IReadOnlyList<Inspection> inspections,
        IReadOnlyList<Employee> employees)
    {
        var treeLookup = trees.ToDictionary(t => t.Id);

        var workOrders = new List<WorkOrder>();

        foreach (var inspection in inspections)
        {
            if (!ShouldCreateWorkOrder(inspection.ObservedHealth))
            {
                continue;
            }

            var tree = treeLookup[inspection.TreeId];

            var assignedEmployee =
                employees[Random.Next(employees.Count)];

            workOrders.Add(
                CreateWorkOrder(
                    tree,
                    inspection,
                    assignedEmployee));
        }

        return workOrders;
    }

    private static bool ShouldCreateWorkOrder(
        TreeHealthStatus health)
    {
        var roll = Random.NextDouble();

        return health switch
        {
            TreeHealthStatus.Excellent => false,

            TreeHealthStatus.Good =>
                roll < 0.10,

            TreeHealthStatus.Fair =>
                roll < 0.45,

            TreeHealthStatus.Poor =>
                true,

            TreeHealthStatus.Dead =>
                true,

            _ => false
        };
    }

    private static WorkOrder CreateWorkOrder(
        Tree tree,
        Inspection inspection,
        Employee employee)
    {
        var createdDate = inspection.InspectionDate;

        var dueDate = inspection.ObservedHealth switch
        {
            TreeHealthStatus.Dead =>
                createdDate.AddDays(7),

            TreeHealthStatus.Poor =>
                createdDate.AddDays(30),

            TreeHealthStatus.Fair =>
                createdDate.AddDays(90),

            _ =>
                createdDate.AddDays(180)
        };

        return WorkOrder.CreateSeedData(
            tree,
            inspection,
            employee,
            DescriptionFor(inspection.ObservedHealth),
            createdDate,
            dueDate,
            WorkOrderStatus.Open);
    }

    private static string DescriptionFor(
        TreeHealthStatus health)
    {
        return health switch
        {
            TreeHealthStatus.Good =>
                "Routine preventative maintenance.",

            TreeHealthStatus.Fair =>
                "Prune tree and remove dead branches.",

            TreeHealthStatus.Poor =>
                "Corrective pruning and structural assessment required.",

            TreeHealthStatus.Dead =>
                "Remove hazardous dead tree.",

            _ =>
                "Maintenance required."
        };
    }
}