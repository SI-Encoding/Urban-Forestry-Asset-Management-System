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
        return await _client.GetFeaturesAsync(
            cancellationToken);
    }
}