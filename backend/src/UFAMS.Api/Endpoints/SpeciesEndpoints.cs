using UFAMS.Application.Features.Species.GetSpecies;

namespace UFAMS.Api.Endpoints;

public static class SpeciesEndpoints
{
    public static IEndpointRouteBuilder MapSpeciesEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/species",
            async (
                GetSpeciesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetSpeciesQuery(),
                    cancellationToken);

                return Results.Ok(response);
            });

        return app;
    }
}