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
                var response = await handler.Handle(
                    new GetParksQuery(),
                    cancellationToken);

                return Results.Ok(response);
            });

        return app;
    }
}