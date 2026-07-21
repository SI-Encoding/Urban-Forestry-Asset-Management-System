using UFAMS.Application.Features.Trees.RegisterTree;
using UFAMS.Application.Features.Trees.GetTrees;
using UFAMS.Application.Features.Trees.GetTree;
namespace UFAMS.Api.Endpoints;

public static class TreeEndpoints
{
    public static IEndpointRouteBuilder MapTreeEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/trees",
            async (
                RegisterTreeCommand command,
                RegisterTreeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    command,
                    cancellationToken);

                return Results.Created(
                    $"/trees/{response.Id}",
                    response);
            });

        app.MapGet(
            "/trees",
            async (
                GetTreesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreesQuery(),
                    cancellationToken);

                return Results.Ok(response);
            });

        app.MapGet(
            "/trees/{id:guid}",
            async (
                Guid id,
                GetTreeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(
                    new GetTreeQuery(id),
                    cancellationToken);

                return Results.Ok(response);
            });

        return app;
    }
}