using UFAMS.Application.Features.Inspections.CreateInspection;
using UFAMS.Application.Features.Inspections.GetInspection;
using UFAMS.Application.Features.Inspections.GetTreeInspections;
using UFAMS.Application.Features.Inspections.UpdateNotes;
using UFAMS.Application.Features.Inspections.UpdateRecommendation;
using UFAMS.Application.Features.Inspections.ScheduleFollowUp;
using UFAMS.Application.Features.Inspections.GetInspections;
namespace UFAMS.Api.Endpoints;

public static class InspectionEndpoints
{
    public static IEndpointRouteBuilder MapInspectionEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/trees/{treeId:guid}/inspections",
            async (
                Guid treeId,
                CreateInspectionCommand command,
                CreateInspectionHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    treeId,
                    command,
                    cancellationToken);

                return Results.Created(
                    $"/inspections/{response.Id}",
                    response);
            })
        .WithName("CreateInspection")
        .WithSummary("Creates a new inspection for a tree")
        .WithDescription(
            "Creates a new inspection record for a specific tree identified by its unique ID.");


        app.MapGet(
            "/trees/{treeId:guid}/inspections",
            async (
                Guid treeId,
                GetTreeInspectionsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreeInspectionsQuery(treeId),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetTreeInspections")
        .WithSummary("Returns inspections for a specific tree")
        .WithDescription(
            "Retrieves all inspection records for a specific tree identified by its unique ID.");

        app.MapGet(
            "/inspections",
            async (
                GetInspectionsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetInspectionsQuery(),
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapGet(
            "/inspections/{id:guid}",
            async (
                Guid id,
                GetInspectionHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetInspectionQuery(id),
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetInspection")
        .WithSummary("Returns a specific inspection")
        .WithDescription(
            "Retrieves the details of a specific inspection identified by its unique ID.");

        app.MapPut(
            "/inspections/{id:guid}/notes",
            async (
                Guid id,
                UpdateInspectionNotesCommand command,
                UpdateInspectionNotesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("UpdateInspectionNotes")
        .WithSummary("Updates inspection notes")
        .WithDescription(
            "Updates the notes for a specific inspection identified by its unique ID.");

        app.MapPut(
            "/inspections/{id:guid}/recommendation",
            async (
                Guid id,
                UpdateInspectionRecommendationCommand command,
                UpdateInspectionRecommendationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("UpdateInspectionRecommendation")
        .WithSummary("Updates inspection recommendation")
        .WithDescription(
            "Updates the recommendation for a specific inspection identified by its unique ID.");

        app.MapPut(
            "/inspections/{id:guid}/follow-up",
            async (
                Guid id,
                ScheduleFollowUpCommand command,
                ScheduleFollowUpHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    id,
                    command,
                    cancellationToken);

                return Results.Ok(response);
            })
        .WithName("ScheduleFollowUp")
        .WithSummary("Schedules a follow-up for an inspection")
        .WithDescription(
            "Schedules a follow-up action for a specific inspection identified by its unique ID.");

        return app;
    }
}