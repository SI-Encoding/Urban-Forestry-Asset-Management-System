using Microsoft.Extensions.Options;
using UFAMS.Infrastructure.ArcGIS;
using UFAMS.Infrastructure.Configuration;

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
            "/arcgis/layer-info",
            async (
                IArcGisAuthenticationService authService,
                IOptions<ArcGisOptions> options,
                HttpClient httpClient,
                CancellationToken cancellationToken) =>
            {
                var token =
                    await authService.GetAccessTokenAsync(
                        cancellationToken);

                var url =
                    $"{options.Value.FeatureServiceUrl}/0" +
                    "?f=pjson" +
                    $"&token={token}";


                var response =
                    await httpClient.GetStringAsync(
                        url,
                        cancellationToken);


                return Results.Content(
                    response,
                    "application/json");
            });

        return app;
    }
}