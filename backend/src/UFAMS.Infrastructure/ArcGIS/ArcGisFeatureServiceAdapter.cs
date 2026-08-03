using UFAMS.Infrastructure.ArcGIS.Models;

namespace UFAMS.Infrastructure.ArcGIS;

public sealed class ArcGisFeatureServiceAdapter
    : IArcGisFeatureProvider
{
    private readonly IArcGisFeatureServiceClient _client;

    public ArcGisFeatureServiceAdapter(
        IArcGisFeatureServiceClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<ArcGisFeature>> GetFeaturesAsync(
        CancellationToken cancellationToken = default)
    {
        //
        // TODO:
        // Replace this with real ArcGIS Feature Service
        // feature retrieval once authentication is complete.
        //
        // The adapter exists so the rest of UFAMS never
        // depends directly on ArcGIS REST models.
        //

        await Task.CompletedTask;

        return
        [
            new ArcGisFeature(
                Id: "1",
                AssetTag: "TREE-0001",
                Species: "Douglas Fir",
                Park: "Stanley Park",
                HealthStatus: "Excellent",
                Latitude: 49.3000,
                Longitude: -123.1400),

            new ArcGisFeature(
                Id: "2",
                AssetTag: "TREE-0002",
                Species: "Bigleaf Maple",
                Park: "Queen Elizabeth Park",
                HealthStatus: "Poor",
                Latitude: 49.2410,
                Longitude: -123.1120),

            new ArcGisFeature(
                Id: "3",
                AssetTag: "TREE-0003",
                Species: "Western Red Cedar",
                Park: "Jericho Park",
                HealthStatus: "Good",
                Latitude: 49.2720,
                Longitude: -123.2020)
        ];
    }
}