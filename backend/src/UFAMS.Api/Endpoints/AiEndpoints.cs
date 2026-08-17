using Microsoft.AspNetCore.Http.HttpResults;

using UFAMS.Application.AI;
using UFAMS.Application.Interfaces;

using UFAMS.Domain.Entities;

namespace UFAMS.Api.Endpoints;

public static class AiEndpoints
{
    public static void MapAiEndpoints(
        this WebApplication app)
    {
        app.MapPost(
            "/api/ai/tree-summary/{treeId:guid}",
            async Task<
                Results<
                    Ok<AiTreeSummaryResponse>,
                    NotFound,
                    BadRequest<string>
                >
            >(
                Guid treeId,
                ITreeRepository treeRepository,
                IInspectionRepository inspectionRepository,
                IWorkOrderRepository workOrderRepository,
                IAiService aiService,
                CancellationToken cancellationToken) =>
            {
                if (treeId == Guid.Empty)
                {
                    return TypedResults.BadRequest(
                        "Invalid tree ID.");
                }

                var tree =
                    await treeRepository.GetByIdAsync(
                        treeId,
                        cancellationToken);

                if (tree is null)
                {
                    return TypedResults.NotFound();
                }

                var inspections =
                    await inspectionRepository
                        .GetByTreeIdAsync(
                            treeId,
                            cancellationToken);

                var workOrders =
                    await workOrderRepository
                        .GetByTreeIdAsync(
                            treeId,
                            cancellationToken);

                var prompt =
                    BuildPrompt(
                        tree,
                        inspections,
                        workOrders);

                var summary =
                    await aiService
                        .GenerateTreeSummaryAsync(
                            prompt,
                            cancellationToken);

                return TypedResults.Ok(
                    new AiTreeSummaryResponse(
                        tree.AssetTag,
                        summary));
            });
    }

    private static string BuildPrompt(
        Tree tree,
        IEnumerable<Inspection> inspections,
        IEnumerable<WorkOrder> workOrders)
    {
        var today =
            DateOnly.FromDateTime(
                DateTime.Today);

        var treeAge =
            CalculateTreeAge(
                tree.PlantingDate,
                today);

        var inspectionList =
            inspections
                .OrderByDescending(
                    inspection =>
                        inspection.InspectionDate)
                .ToList();

        var workOrderList =
            workOrders
                .OrderByDescending(
                    workOrder =>
                        workOrder.CreatedDate)
                .ToList();

        var inspectionSummary =
            inspectionList.Count == 0
                ? "No inspections recorded."
                : string.Join(
                    Environment.NewLine,
                    inspectionList.Select(
                        inspection =>
                            $"""
                            - Inspection Date:
                              {inspection.InspectionDate}

                              Observed Health:
                              {inspection.ObservedHealth}

                              Notes:
                              {inspection.Notes}

                              Recommendation:
                              {inspection.Recommendation}

                              Next Inspection Date:
                              {inspection.NextInspectionDate?.ToString()
                               ?? "Not scheduled"}
                            """));

        var workOrderSummary =
            workOrderList.Count == 0
                ? "No work orders recorded."
                : string.Join(
                    Environment.NewLine,
                    workOrderList.Select(
                        workOrder =>
                        {
                            var overdue =
                                workOrder.DueDate.HasValue &&
                                workOrder.DueDate.Value < today &&
                                workOrder.CompletedDate is null &&
                                workOrder.Status !=
                                    Domain.Enums.WorkOrderStatus.Cancelled;

                            var overdueStatus =
                                overdue
                                    ? "OVERDUE"
                                    : "Not overdue";

                            return
                                $"""
                                - Status:
                                  {workOrder.Status}

                                  Description:
                                  {workOrder.Description}

                                  Due Date:
                                  {workOrder.DueDate?.ToString()
                                   ?? "No due date"}

                                  Completed Date:
                                  {workOrder.CompletedDate?.ToString()
                                   ?? "Not completed"}

                                  Due Date Status:
                                  {overdueStatus}
                                """;
                        }));

        var latestInspection =
            inspectionList.FirstOrDefault();

        var latestInspectionSummary =
            latestInspection is null
                ? "No inspections recorded."
                :
                $"""
                Date:
                {latestInspection.InspectionDate}

                Observed Health:
                {latestInspection.ObservedHealth}

                Notes:
                {latestInspection.Notes}

                Recommendation:
                {latestInspection.Recommendation}

                Next Inspection Date:
                {latestInspection.NextInspectionDate?.ToString()
                 ?? "Not scheduled"}
                """;

        var openWorkOrders =
            workOrderList
                .Where(
                    workOrder =>
                        workOrder.Status !=
                            Domain.Enums.WorkOrderStatus.Completed &&
                        workOrder.Status !=
                            Domain.Enums.WorkOrderStatus.Cancelled)
                .ToList();

        var overdueWorkOrders =
            openWorkOrders
                .Where(
                    workOrder =>
                        workOrder.DueDate.HasValue &&
                        workOrder.DueDate.Value < today)
                .ToList();

        return
            $"""
            You are analyzing an urban forest tree
            for the UFAMS asset management system.

            Use ONLY the information provided below.

            Do not invent facts.

            IMPORTANT:
            UFAMS has already calculated deterministic
            values such as tree age and work order
            overdue status.

            Do NOT recalculate these values yourself.

            Treat the supplied values as authoritative.

            Current Date:
            {today}

            --------------------------------------------------
            TREE ASSET
            --------------------------------------------------

            Asset Tag:
            {tree.AssetTag}

            Health Status:
            {tree.HealthStatus}

            Height:
            {tree.HeightInMeters} meters

            Diameter:
            {tree.DiameterInCentimeters} centimeters

            Planting Date:
            {tree.PlantingDate}

            Calculated Tree Age:
            {treeAge} years

            Latitude:
            {tree.Location.Latitude}

            Longitude:
            {tree.Location.Longitude}

            --------------------------------------------------
            LATEST INSPECTION
            --------------------------------------------------

            {latestInspectionSummary}

            --------------------------------------------------
            INSPECTION HISTORY
            --------------------------------------------------

            {inspectionSummary}

            --------------------------------------------------
            WORK ORDER SUMMARY
            --------------------------------------------------

            Total Work Orders:
            {workOrderList.Count}

            Open Work Orders:
            {openWorkOrders.Count}

            Overdue Open Work Orders:
            {overdueWorkOrders.Count}

            --------------------------------------------------
            WORK ORDER DETAILS
            --------------------------------------------------

            {workOrderSummary}

            --------------------------------------------------
            TASK
            --------------------------------------------------

            Provide a concise assessment of this tree.

            Organize the response into:

            Summary

            Risk Assessment

            Recommended Actions

            Consider:

            - Current tree health
            - Tree age
            - Inspection history
            - Latest inspection
            - Inspection recommendations
            - Upcoming inspections
            - Existing work orders
            - Work order status
            - Whether work orders are overdue
            - Whether additional maintenance may be appropriate

            IMPORTANT RULES:

            1. Do not invent facts.

            2. Do not claim that a problem exists unless
               the supplied data supports that conclusion.

            3. Do not describe an old work order as upcoming
               if its due date has already passed.

            4. If an open work order is overdue, clearly
               identify it as overdue.

            5. If a work order is already completed, do not
               recommend completing it again.

            6. Do not invent inspection dates,
               maintenance history, diseases, defects,
               hazards, or environmental conditions.

            7. Do not assume that a tree is hazardous merely
               because of its age or size.

            8. Distinguish between documented facts and
               professional recommendations.

            9. If the available information is insufficient
               to make a recommendation, explicitly say so.

            10. Recommendations should support municipal
                urban forestry decision-making and should
                not replace assessment by qualified personnel.
            """;
    }

    private static int CalculateTreeAge(
        DateOnly plantingDate,
        DateOnly today)
    {
        var age =
            today.Year -
            plantingDate.Year;

        if (plantingDate.AddYears(age) > today)
        {
            age--;
        }

        return Math.Max(age, 0);
    }
}

public sealed record AiTreeSummaryResponse(
    string AssetTag,
    string Summary);