using UFAMS.Application.Features.Parks.GetParkInventory;
using UFAMS.Application.Features.Parks.GetParks;

namespace UFAMS.Api.Endpoints;

public static class ParkEndpoints
{
    public static IEndpointRouteBuilder MapParkEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/parks",
            async (
                GetParksHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        new GetParksQuery(),
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetParks")
        .WithSummary("Returns all parks")
        .WithDescription(
            "Retrieves all parks managed by the Urban Forest Asset Management System.");

        app.MapGet(
            "/parks/{parkId:guid}/inventory",
            async (
                Guid parkId,
                GetParkInventoryHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        new GetParkInventoryQuery(parkId),
                        cancellationToken);

                return Results.Ok(response);
            })
        .WithName("GetParkInventory")
        .WithSummary("Returns park inventory")
        .WithDescription(
            "Provides tree counts, health statistics, and species breakdown for a park.");

        return app;
    }
}