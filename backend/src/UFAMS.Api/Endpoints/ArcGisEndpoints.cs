using UFAMS.Infrastructure.ArcGIS;

namespace UFAMS.Api.Endpoints;

public static class ArcGisEndpoints
{
    public static IEndpointRouteBuilder MapArcGisEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/arcgis/token-test",
            async (
                IArcGisAuthenticationService authenticationService,
                CancellationToken cancellationToken) =>
            {
                var token =
                    await authenticationService.GetAccessTokenAsync(
                        cancellationToken);

                return Results.Ok(
                    new
                    {
                        Success = true,
                        TokenLength = token.Length
                    });
            })
        .WithName("ArcGisTokenTest")
        .WithSummary("Tests ArcGIS OAuth authentication")
        .WithDescription(
            "Requests an OAuth token from ArcGIS Online.");

        return app;
    }
}