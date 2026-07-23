using UFAMS.Application.Features.Species.GetSpecies;
using UFAMS.Application.Features.Species.SearchSpecies;
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

        app.MapGet(
            "/species/search",
            async (
                string query,
                SearchSpeciesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response =
                    await handler.Handle(
                        new SearchSpeciesQuery(query),
                        cancellationToken);

                return Results.Ok(response);
            });

        return app;
    }
}