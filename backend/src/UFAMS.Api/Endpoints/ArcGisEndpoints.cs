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

                return Results.Ok(new
                {
                    Success = true,
                    TokenLength = token.Length
                });
            });

        app.MapGet(
            "/arcgis/service-info",
            async (
                IArcGisFeatureServiceClient client,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await client.GetServiceInfoAsync(
                        cancellationToken);

                return Results.Ok(result);
            });

        app.MapGet(
            "/arcgis/features",
            async (
                IArcGisFeatureServiceClient client,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await client.GetFeaturesAsync(
                        cancellationToken);

                return Results.Ok(result);
            });

        app.MapGet(
            "/arcgis/raw-features",
            async (
                IArcGisFeatureServiceClient client,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await client.GetRawFeaturesAsync(
                        cancellationToken);

                return Results.Text(
                    result,
                    "application/json");
            });

        return app;
    }
}